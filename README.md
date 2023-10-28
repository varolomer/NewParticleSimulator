# C# OpenGL Newtonian-Particle-Simulator-Enhancement
![newparticlesim](https://github.com/varolomer/NewParticleSimulator/assets/42884775/a3de15b7-d55a-4e0e-83fe-3dea44e95ad7)

[Video Youtube Link](https://youtu.be/cyBUFOMkRqc?si=TUfU5eihWQGMKel0)

## Introduction
This library is a product of studying "BoyBayKiller"s [Newtonian-Particle-Simulator](https://github.com/BoyBaykiller/Newtonian-Particle-Simulator). I will keep enhancing the library in terms of UI and buffer management while adding more primitives dynamically.

Honestly, I feel much more comfortable managing the buffers in OpenGL with C++ code but for some ongoing projects, I will brush up my C# codebase for OpenGL as well. I will re-implement my C++ libraries in C# using OpenTK. Hopefully, if I can find enough space I will bring up some more repositories.

## User Interface
I added a user interface using .NET port of Dear ImGui. Later on, I will change the sample interface and bind the UI input directly to Input Manager static class. I am also developing A WPF and WinForms, later on if I have time I can release those projects as well.
![image](https://github.com/varolomer/NewParticleSimulator/assets/42884775/c3cc568c-6247-4fba-8fca-0f8d35d8de80)

## Keyboard & Drawing
### When the app is in idling mode it will only draw the particles:
![image](https://github.com/varolomer/NewParticleSimulator/assets/42884775/f70096f5-df69-4316-8b57-8e6e824930c3)

### When left shift key is pressed it will draw Lines (adj) as well:
![newparticlesim](https://github.com/varolomer/NewParticleSimulator/assets/42884775/cf30d3b5-e6ae-409b-a2a7-6f13d8a92b77)

### Then left ctrl key is pressed it will draw Triangle Strips (adj) instead of lines:
![TriStripsMin2](https://github.com/varolomer/NewParticleSimulator/assets/42884775/912e8f35-eed1-4e98-a45b-b4c1795dbc1d)

