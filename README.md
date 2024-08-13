# Pibstermatic Sound 2000
Pibstermatic Sound 2000 is a tool for tuning engine sound settings for PiBoSo games. It was written for GP Bikes, but it might work
for MX Bikes, Kart Racing Pro etc.

How to:
Either launch the executable without arguments, then select an engine file (engine.scl or engine_onboard.scl), or launch with the first
command line argument being the full path of the engine file.

1) Select 'Stroke' and 'Cylinders'.
2) Click 'Disable All'.
3) Click 'Edit Envelopes'.
4) Select a sample in the envelope editor.
5) Select 'Proportional to RPM'.
6) On the main form, click 'Extended Range' for the selected sample
7) Pull the RPM slider and try to find a value where you see clear peaks in the FFT view.
8) Tune Reference RPM for the sample so that the vertical lines in the plot coincide with the peaks.
9) Uncheck 'Extended Range'
10) Pick another sample and goto 5.
11) Click 'Enable All'.
12) Pull the RPM and On slider and hopefully observe no pitch shifts between samples.
13) Look at RMS when varying RPM and On. Probably, the value should never go down with increasing RPM, or when On is enabled.
14) Save in the live edit view.

***Peaks****

Cycle: For four stroke engines, this rate has half the rate of revolution. There should never be a lower visible peak than this, 
but it can be very weak - especially for 'off' samples.

Revolution:  The rate of the engine.

Combustion: The rate of the engine, times the number of cylinders, divided by two for four stroke engines.

Cylinder: The rate of the engine times the number of cylinders.

Note that (except for pure sine waves), there will also be peaks at multiples of the base frequency.
