# Changelog
All notable changes to this package will be documented in this file.

## [3.0.0] - 05-02-2026

# Added
- Support for Volumes.

# Removed
- Removed support for 2022.3.

## [2.1.1] - 26-12-2025

### New
- 'Use scaled time' in Advanced settings.

## [2.1.0] - 02-09-2025

### Added
- 18 new film manufacturers with Color Decision List (CDL) characteristics:
  - **Kodak Modern Films**: Ektar 100, Portra 160/400/800, Ektachrome E100
  - **Fujifilm**: Pro 400H, Velvia 50/100, Provia 100F
  - **Ilford B&W**: HP5 Plus, FP4 Plus, Delta 100/400
  - **Cinestill**: 50D (daylight), 800T (tungsten)
  - **Lomography**: Color Negative 100/400
  - **Ferrania**: P30 (Italian B&W)

### Fix
- Fixed shader compilation and build issues.
- Fixed VR build compatibility in Unity 6.

## [2.0.2] - 24-04-2025

# Fix
- Camera 'Post Processing' checkbox fixed.

## [2.0.1] - 11-03-2025

### Fix
- Color precision error.

## [2.0.0] - 04-03-2025

### New
- Support for Unity 6 Render Graph.
- Support for effects in multiples Renderers.

### Removed
- Removed GetSettings(), use .Instance.settings

### Fix
- Small fixes.

## [1.4.0] - 19-10-2024

# Changed
- Support for Unity 2022.3.45f1 LTS and Unity 6000.0.23f1 LTS.
- Updated to Universal RP 14.0.11.
- Removed support for Unity 2021.3 LTS.
- Performance improvements.

## [1.3.0] - 17-07-2024

# Changed
- Removed support for Built-In.
- Performance improvements.

# Fix
- Small fixes.

## [1.2.0] - 20-06-2024

### Fixed
- Build stuck when modifying the URP Pipeline Asset file in runtime.

### Changed
- Removed IsInRenderFeatures(), AddRenderFeature(), IsEnable() and SetEnable() functions.

### Added
- Update checker.

## [1.1.1] - 02-06-2024

### Changed
- New online documentation.

## [1.1.0] - 05-10-2023

### Added
- Built-in renderer support.

### Changed
- RTHandle use in Unity 2022.1 or newer.
- ProfilingScope use in Unity 2022.1 or newer.

### Fixed
- Removed warning messages of deprecated functions in Unity 2022.1 or newer.

## [1.0.1] - 30-05-2023

### Fixed
- Chrome and Edge WebGL fix.
- VR fix.

## [1.0.0] - 18-12-2022

- Initial release.