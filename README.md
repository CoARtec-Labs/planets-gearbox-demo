# Planets Gearbox Demo

Demo of a digital assembly manual for a toy geabox based on Unity. The app features:
- step-by-step assembly instructions of the gearbox
- visualization of the assembly part by AR
- state machine of the UI over multiple scenes
- object detection of the parts with a custom-trained model
- deployable to desktop or to mobile devices (Android, iPhone) for AR mode

This demo is built around the planetary gearbox design by Lama2008 on [Thingiverse](https://www.thingiverse.com/thing:6106552), which can be downloaded and printed DIY.

This project is licensed under GNU GENERAL PUBLIC LICENSE V3.

## Installation

Clone this package as well as [barracuda-yolo-testbed](https://github.com/CoARtec-Labs/barracuda-yolo-testbed) next to each other, ie. into the same workspace, and on the main branch (latest commit most likely). All nested dependendencies will then be located automatically.

In order to run the demo, scenes Main and Instructor must be loaded in the scene tree or deployed to the device. 

## Credits

The gearbox design by Lama2008: https://www.thingiverse.com/thing:6106552

Unity packages for object detection by C. Mills: https://github.com/cj-mills/

State machine pattern for Unity by Patryk Galach: https://www.patrykgalach.com/2019/03/18/design-pattern-state-machine/

