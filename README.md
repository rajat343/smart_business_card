# Smart Business Card

A Unity-based Augmented Reality (AR) application designed to enhance the functionality of 
traditional business cards. Using image recognition and AR technology, this project provides 
a more interactive and accessible way to access and expand on information from business cards.

## Table of Contents

- Introduction
- Features
- Technology Stack
- Installation
- Usage
- Potential Applications
- Future Enhancements
- Contributing
- License


## Introduction

Traditional business cards are limited by size, lack of interactivity, and accessibility 
challenges, especially for people with vision impairments. This project aims to modernize 
the business card experience by integrating AR technology. By scanning a business card with 
a mobile device, users can access extended information and explore interactive 3D content, 
creating a richer and more accessible experience.

The application uses Unity and Vuforia for image recognition and AR rendering, enabling 
users to view additional business information and interact with virtual objects associated 
with the card.


## Features

- **Enhanced Information Access**: Unlock additional information beyond the static text 
  on a business card.
- **Virtual 3D Map**: View location-based information and maps through AR directly on the 
  business card.
- **Industry-Specific Functionalities**:
  - Builders can showcase virtual tours of buildings or construction sites.
  - Showroom owners can provide interactive 3D views of products, allowing customers to 
    explore options like color and features.
- **AR Marker Recognition**: Business cards serve as AR markers, enabling easy and accurate 
  information retrieval.
- **Unity and Vuforia Integration**: The application is built on Unity and uses the Vuforia 
  library for image recognition and AR tracking.


## Technology Stack

- **Unity**: Game engine used for creating and rendering the AR experience.
- **Vuforia**: AR platform for marker-based tracking and image recognition.
- **C#**: Primary programming language for scripting in Unity.
- **JavaScript**: Used for additional scripting.
- **Image Processing**: Processes business card images for AR marker recognition and 
  content augmentation.



## Installation

### Prerequisites

- **Unity**: Download and install [Unity Hub](https://unity.com/download) and select a 
  compatible Unity version (unity version 6 or above).
- **Vuforia SDK**: Install the Vuforia Engine from the Unity Asset Store or through the 
  Vuforia Developer Portal.


### Steps

```markdown
1. Clone the Repository:
   git clone https://github.com/rajat343/ar_business_card.git
   cd ar_business_card


2. Open in Unity:
   - Open Unity Hub.
   - Select "Add Project" and navigate to the `smart_business_card` folder to open the project in Unity.


3. Setup Vuforia:
   - Register on the [Vuforia Developer Portal](https://developer.vuforia.com/) and obtain a license key.
   - In Unity, go to `Window` > `Vuforia Configuration` and enter your license key.


4. Build Settings:
   - Open `File` > `Build Settings`.
   - Select your target platform (e.g., Android or iOS).
   - Ensure that Vuforia Augmented Reality Support is enabled under `Player Settings`.


5. Run the Project:
   - Connect your mobile device or use an emulator.
   - Click on the "Play" button in Unity to test the app, or build and deploy it to a device for a full experience.



