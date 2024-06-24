
# Thal-Val

Unity implementation of <a  href="10.1038/nature04766"  target="_blank">Daw's Four-Armed Restless Bandit</a>; a reinforcement learning task used recently in a <a  href="https://doi.org/10.1093/brain/awae025"  target="_blank"> study assessing value-based decision-making in Parkinson’s disease apathy  </a> (see press release <a  href="https://www.dundee.ac.uk/stories/parkinsons-patients-work-their-brains-harder-stay-motivated"  target="_blank"> here </a> for summary.

## Description
Intending to be a portable; OS/ device agnostic task.
Builds ok for iOS & WebGL; others in testing.
Original motivation was to deliver task on iPad; see  <a  href="https://gilbertson2209.github.io/thal-val/"> WebGL build</a>.

## Getting Started
To build; download Unity; and import project from here; and build yourself; will report on issues as tested.
The task works but source code quite poorly abstracted around use case (i.e. var names/ methods don't match psych task terminology).
Task in main doesn't match Daw's Restless Bandit params at the moment as was built for speed (things like 'ANIMATE_TIME' & 'INTERTRIAL INTERVAL' have been reduced & fixed).

### Dependencies
Unity 2022.3.16f1; 2D (URP)
(Packages in package list currently not clean and include packages from other projects)

### Builds & Installing
Follow Unity Build Processes but just ask if help needed; always happy.

### Logs, Data
Currently the device builds should read out data on the [Unity Application Persistent Data Path](https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html) but doesn't seem to work on WebGL. You can use it now as the results are read out onto the dev console on a browser but recognise this needs better solution.
## Help
For code/ build get in touch with developer; via [@i-brnrd](https://github.com/i-brnrd) on here or via [University of Dundee](https://www.dundee.ac.uk/people/isla-barnard); for info on actual use of the task etc for psych research contact [here](https://www.dundee.ac.uk/people/tom-gilbertson).

## Authors
Isla Barnard (all code & sprites)
Graham Mackenzie, Will Gilmour, Tom Gilbertson

## Acknowledgments
To [Nathanial Daw](https://dawlab.princeton.edu) for <a  href="10.1038/nature04766"  target="_blank">Daw's Four-Armed Restless Bandit</a>

## License
See [here](/LICENCE).
