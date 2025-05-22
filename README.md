# Planets Gearbox Demo

This is COARtec's planetary gearbox demo in unity. It features: 
- step-by-step assembly of the gearbox
- state machine of the UI over multiple scenes
- object detection of the parts with an custom trained onnx model
- deployable to the iPhone only so far (AVP coming soon)

This demo is built around the planetary gearbox design by Lama2008 on [Thingiverse](https://www.thingiverse.com/thing:6106552), which can be downloaded and DIY be printed.

## Installation

You need to install CoARtec's YOLO Interface package which is part of the Barracuda YOLO Testbed.

Clone [barracuda-yolo-testbed](https://github.com/jonni-max/barracuda-yolo-testbed.git) to the same workspace folder where this package is located. 
The nested dependent packages will be located automatically.

## Credits

The gearbox design by Lama2008 (https://www.thingiverse.com/thing:6106552)

Unity packages for object detection by C. Mills (https://github.com/cj-mills/unity-barracuda-inference-yolox)

State machine pattern by PATRYK GALACH (https://www.patrykgalach.com/2019/03/18/design-pattern-state-machine/)
