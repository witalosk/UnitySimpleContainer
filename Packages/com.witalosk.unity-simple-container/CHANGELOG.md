# Changelog

## [1.1.0] - 2024-03-15
### Change the directory structure.
- The URL for installation in the Package Manager will change accordingly.
### Add a IContainer.BindAsTransient method.
- This method binds a type to a new instance of itself every time it is resolved.
### Add an extension method to Resolve and AddComponent.
- This method resolves a type and adds a component to the GameObject.
- Only works with MonoBehaviour types and BindAsTransient method.

## [1.0.1] - 2023-09-15
### Change the sample project container name.
- to avoid conflict with your projects.

## [1.0.0] - 2023-09-15
### This is the first release of *Unity Package Unity Simple Container*.
- Added support for Unity Package Manager.
