# C# OpenGL Newtonian-Particle-Simulator-Enhancement
![newparticlesim](https://github.com/varolomer/NewParticleSimulator/assets/42884775/c3c0898f-30c5-4f5c-820f-ffc27045c11a)

[Video Youtube Link](https://youtu.be/cyBUFOMkRqc?si=TUfU5eihWQGMKel0)

## Introduction
This library is a product of studying "BoyBayKiller"s [Newtonian-Particle-Simulator](https://github.com/BoyBaykiller/Newtonian-Particle-Simulator). I will keep enhancing the library in terms of UI and buffer management while adding more primitives dynamically.

Honestly, I feel much more comfortable managing the buffers in OpenGL with C++ code but for some ongoing projects, I will brush up my C# codebase for OpenGL as well. I will re-implement my C++ libraries in C# using OpenTK. Hopefully, if I can find enough space I will bring up some more repositories.

## User Interface
I added a user interface using .NET port of Dear ImGui. Later on, I will change the sample interface and bind the UI input directly to Input Manager static class. I am also developing A WPF and WinForms, later on if I have time I can release those projects as well.

![image](https://github.com/varolomer/NewParticleSimulator/assets/42884775/81003f2e-194a-4f1d-9ae5-1f3caaa59ee6)

## Keyboard & Drawing
When the app is in idling mode it will only draw the particles:
![278778999-76312266-0b66-460e-8284-103a141699f5](https://github.com/varolomer/NewParticleSimulator/assets/42884775/414c9f77-d5d8-426b-b8c4-d1e9af96b52b)

When left shift key is pressed it will draw Lines (adj) as well:
![TriStripsMin](https://github.com/varolomer/NewParticleSimulator/assets/42884775/f43e6576-0aee-4524-a539-ea893e6efa7a)

Then left ctrl key is pressed it will draw Triangle Strips (adj) instead of lines:
![TriStripsMin2](https://github.com/varolomer/NewParticleSimulator/assets/42884775/2778893c-3a5a-46d5-9cf2-a699f4800983)
