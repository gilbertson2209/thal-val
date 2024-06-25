
# Thal-Val

Unity implementation of <a  href="10.1038/nature04766"  target="_blank">Daw's Four-Armed Restless Bandit</a>; a reinforcement learning task used recently in a <a  href="https://doi.org/10.1093/brain/awae025"  target="_blank"> study assessing value-based decision-making in Parkinson’s disease apathy  </a> (see press release <a  href="https://www.dundee.ac.uk/stories/parkinsons-patients-work-their-brains-harder-stay-motivated"  target="_blank"> here </a> for summary.

## Description
Intending to be a portable; OS/ device agnostic task.

Builds ok for iOS & WebGL; others in testing.

Original motivation was to deliver task on iPad; see <a  href="https://gilbertson2209.github.io/thal-val/"> WebGL build</a> for a demo/ to play through.

## Getting Started
To build; download Unity; and import & build. This is not tested; will report on issues as they come up.

The task works; c# script structure doesn't meet use case (i.e. var names/ methods don't match psych task terminology).

Task in main doesn't match Daw's Restless Bandit params at the moment as was built for speed (things like 'ANIMATE_TIME' & 'INTERTRIAL INTERVAL' have been reduced & fixed) but these are exposed in the inspector.

### Dependencies
Unity 2022.3.16f1; 2D (URP)
(Packages in package list currently not clean and include packages from other projects)

### Builds & Installing
Follow Unity Build Processes but just ask if help needed; always happy.

### Logs, Data
Currently the device builds should read out data on the [Unity Application Persistent Data Path](https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html); however the WebGL idbfs doesn't seem to expose the data. The results are read out onto the dev console on a browser but recognise this needs better solution.
## Help
For code/ build get in touch with developer; via [@i-brnrd](https://github.com/i-brnrd) on here or via [University of Dundee](https://www.dundee.ac.uk/people/isla-barnard).
For details on how to use task in research, try [here](https://www.dundee.ac.uk/people/tom-gilbertson) instead.

## Authors
Isla Barnard

Graham Mackenzie, Will Gilmour, Tom Gilbertson

## Acknowledgments
To [Nathanial Daw](https://dawlab.princeton.edu) for <a  href="10.1038/nature04766"  target="_blank">Daw's Four-Armed Restless Bandit</a>

## License
See [here](/LICENCE).
