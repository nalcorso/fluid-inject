<div id="top"></div>
<!--
*** Thanks for checking out the Best-README-Template. If you have a suggestion
*** that would make this better, please fork the repo and create a pull request
*** or simply open an issue with the tag "enhancement".
*** Don't forget to give the project a star!
*** Thanks again! Now go create something AMAZING! :D
-->



<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]



<!-- PROJECT LOGO -->
<!--
<br />
<div align="center">
  <a href="https://github.com/nalcorso/fluid-inject">
    <img src="images/logo.png" alt="Logo" width="80" height="80">
  </a>
-->

<h3 align="center">FLUiD Inject</h3>

  <p align="center">
    A very simple and lightweight injection framework for .NET
    <br />
    <a href="https://github.com/nalcorso/fluid-inject"><strong>Explore the docs »</strong></a>
    <br />
    <br />
    <a href="https://github.com/nalcorso/fluid-inject">View Demo</a>
    ·
    <a href="https://github.com/nalcorso/fluid-inject/issues">Report Bug</a>
    ·
    <a href="https://github.com/nalcorso/fluid-inject/issues">Request Feature</a>
  </p>
</div>



<!-- TABLE OF CONTENTS -->
<!--
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>
-->


<!-- ABOUT THE PROJECT -->
## About The Project
<!--
[![Product Name Screen Shot][product-screenshot]](https://example.com)
-->
This project is a simple injection framework for .NET. It was designed to learn about the internals of a DI framework and the Expression API.
There is no legitimate reason to use this in any kind of production environment. This project is worse then other DI frameworks in every way!
It is slower, uses more memory, has less features.

<p align="right">(<a href="#top">back to top</a>)</p>

### Mature DI Frameworks you should use instead
* [AutoFac](https://autofac.org)
* [Microsoft.Extensions.DependencyInjection](https://dotnet.microsoft.com/en-us/)

<p align="right">(<a href="#top">back to top</a>)</p>


### Built With

* [.NET](https://dotnet.microsoft.com/en-us/)
* [C#](https://docs.microsoft.com/en-us/dotnet/csharp/)

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- GETTING STARTED -->
## Getting Started

This is an example of how you may give instructions on setting up your project locally.
To get a local copy up and running follow these simple example steps.

<!--
### Prerequisites

This is an example of how to list things you need to use the software and how to install them.
* npm
  ```sh
  npm install npm@latest -g
  ```
-->

### Installation

1. Ask yourself why on earth you want to use this software.
2. Clone the repo
   ```sh
   git clone https://github.com/nalcorso/fluid-inject.git
   ```
3. Reference the project in your project file
4. Add the Fluid.Inject namespace to your project file
   ```csharp
   using Fluid.Inject;
   ```

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- USAGE EXAMPLES -->
## Usage

Below is a simple example of using the API. The main difference is that this API does not require a ContainerBuilder.
```csharp
IContainer container = new Container();

container.Add<MyService>().As<IMyService>.AsSingleton();

var my_service = container.Get<MyService>();
```

_For more examples, please refer to the [Documentation](https://example.com)_

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- ROADMAP -->
## Roadmap

- [ ] Add Support for Named Services
- [ ] Add Support for Automatic Scanning for Services in an Assembly
    - [ ] Simple Service Scanning (by Interface)
	- [ ] Service Scanning by Predicate eg. s => s.Type.Name.EndsWith("ViewModel");
    - [ ] Module Loading with self registration / unregistration.
- [ ] Formalise resolution model (eg Order of precedence of objects / constructors)

See the [open issues](https://github.com/nalcorso/fluid-inject/issues) for a full list of proposed features (and known issues).

<p align="right">(<a href="#top">back to top</a>)</p>


<!-- CONTRIBUTING -->
## Contributing

Contributions are welcome. Please consider that this project is not meant to be anything more than a playground. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE` for more information.

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- CONTACT -->
## Contact

Nicholas Alcorso - [@nalcorso](https://twitter.com/nalcorso) - nalcorso@gmail.com

Project Link: [https://github.com/nalcorso/fluid-inject](https://github.com/nalcorso/fluid-inject)

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- ACKNOWLEDGMENTS -->
## Acknowledgments

* [Autofac](https://autofac.org)
* [Best-README-Template](https://github.com/othneildrew/Best-README-Template)

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/nalcorso/fluid-inject.svg?style=for-the-badge
[contributors-url]: https://github.com/nalcorso/fluid-inject/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/nalcorso/fluid-inject.svg?style=for-the-badge
[forks-url]: https://github.com/nalcorso/fluid-inject/network/members
[stars-shield]: https://img.shields.io/github/stars/nalcorso/fluid-inject.svg?style=for-the-badge
[stars-url]: https://github.com/nalcorso/fluid-inject/stargazers
[issues-shield]: https://img.shields.io/github/issues/nalcorso/fluid-inject.svg?style=for-the-badge
[issues-url]: https://github.com/nalcorso/fluid-inject/issues
[license-shield]: https://img.shields.io/github/license/nalcorso/fluid-inject.svg?style=for-the-badge
[license-url]: https://github.com/nalcorso/fluid-inject/blob/master/LICENSE.txt