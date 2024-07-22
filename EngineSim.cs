using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveMix
{
    internal class EngineSim
    {
        enum EState
        {
            Crankshaft,
            // Note: RPMs for Wheel is at the crankshaft as if it were not in neutral - even if it is.
            Wheel,
        }

        enum ERPMUpdateType
        {
            // Engine RPM, in neutral
            Crankshaft = 1 << (int)EState.Crankshaft,
            // Engine RPM as if it's not in neutral - but it is.
            Wheel = 1 << (int)EState.Wheel,
            Both = Crankshaft | Wheel,
        };

        static readonly float c_NormedMass = 100.0f;
        static readonly float c_NormedMassNeutral = 10.0f;
        static readonly float c_WindDrag = 0.04f;
        static readonly float c_EngineDrag = 0.2f;
        static readonly float c_TorqueBrake = 1.0f;

        float m_IdleRPM = 1000;
        float m_MaxRPM = 15000;

        float m_RPM = 1000;
        float m_Speed = 0;
        float m_Gearing = 0;
        float m_Brake = 0;
        bool m_Neutral = false;

        public EngineSim()
        {
        }
        public float MaxRPM
        {
            get { return m_MaxRPM; }
            set { m_MaxRPM = value; }
        }

        public float IdleRPM
        {
            get { return m_IdleRPM; }
            set {
                m_IdleRPM = Math.Clamp(value, 0, 1e6f);
            }
        }

        public float Gearing
        {
            get { return m_Gearing; }
            set {
                m_Gearing = value;
            }
        }

        public float CurrentRPM
        {
            get {
                return GetCurrentRPM(EState.Crankshaft);
            }
            set 
            { 
                m_RPM = value;
                m_Speed = (value/MaxRPM) * GearRatio;
            }
        }

        public float Brake
        {
            get { return m_Brake; }
            set { m_Brake = value; }
        }

        internal float GearRatio
        {
            get
            {
                return (m_Gearing + 1) * 3.0f;
            }
        }

        internal bool Neutral
        {
            get { return m_Neutral; }
            set { m_Neutral = value; }
        }

        float GetCurrentRPM(EState state)
        {
            return (state == EState.Crankshaft) ? m_RPM : (m_Speed * MaxRPM / GearRatio);
        }

        static float EvaluateTorque(float normed_rpm)
        {
            if (normed_rpm > 1.0f)
                return 0;
            return 0.5f + normed_rpm * 0.5f - normed_rpm * normed_rpm * 0.25f;
        }

        struct SRPMUpdate
        {
            public float m_RPM;
            public float m_On;
            public SRPMUpdate(float rpm, float on)
            {
                m_RPM = rpm;
                m_On = on;
            }
        }

        SRPMUpdate UpdateRPM(float dt, float throttle, ERPMUpdateType update_type)
        {
            float curr_rpm = GetCurrentRPM((update_type == ERPMUpdateType.Crankshaft) ? EState.Crankshaft : EState.Wheel);
            float normed_rpm = curr_rpm / MaxRPM;

            // Calculate set point for idle
            float torque_at_idle = EvaluateTorque(IdleRPM / MaxRPM);
            float normed_rpm_idle_want = IdleRPM / MaxRPM;
            float normed_rpm_idle_set = normed_rpm_idle_want * (c_EngineDrag + torque_at_idle) / torque_at_idle;

            float normed_rpm_set = Math.Max(throttle * 3, normed_rpm_idle_set);

            float delta_normed_rpm = Math.Max(normed_rpm_set - normed_rpm, 0);

            float torque_engine_max = EvaluateTorque(normed_rpm);
            float torque_engine = Math.Max(delta_normed_rpm * torque_engine_max, 0);

            float gear_ratio = (update_type == ERPMUpdateType.Crankshaft) ? 0 : GearRatio;
            float speed = normed_rpm * gear_ratio;
            float torque_wind_drag = speed * speed * c_WindDrag;
            float torque_engine_drag = (update_type == ERPMUpdateType.Wheel) ? 0 : (normed_rpm * c_EngineDrag);
            float torque_brake = (update_type == ERPMUpdateType.Crankshaft) ? 0 : (m_Brake * c_TorqueBrake);

            float torque_total = torque_engine - torque_wind_drag - torque_engine_drag - torque_brake;

            float normed_mass = (update_type == ERPMUpdateType.Crankshaft) ? c_NormedMassNeutral : (c_NormedMass * gear_ratio);

            float a = 60.0f * torque_total / normed_mass;       // 60: Forgot to include dt when I tuned the constants :-) .

            float new_rpm = Math.Clamp(curr_rpm + a * MaxRPM * dt, 0, MaxRPM);
            float on = Math.Clamp(delta_normed_rpm, 0, 1);

            return new SRPMUpdate(new_rpm, on);
        }

        internal SEngineState Update(float dt, float throttle)
        {
            SRPMUpdate rpm_update = UpdateRPM(dt, throttle, Neutral ? ERPMUpdateType.Crankshaft : ERPMUpdateType.Both);
            m_RPM = rpm_update.m_RPM;

            if (Neutral)
                m_Speed = (UpdateRPM(dt, 0, ERPMUpdateType.Wheel).m_RPM / MaxRPM) * GearRatio;
            else
                m_Speed= (m_RPM / MaxRPM) * GearRatio;

            SEngineState state;
            state.m_RPM = m_RPM;

            state.m_On = rpm_update.m_On;
            state.m_Off = 1.0f - state.m_On;
            return state;
        }
    }
}
