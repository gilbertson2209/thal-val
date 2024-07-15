
# Thal-Val

Unity implementation of <a  href="10.1038/nature04766"  target="_blank">Daw's Four-Armed Restless Bandit</a>; a reinforcement learning task used recently in a <a  href="https://doi.org/10.1093/brain/awae025"  target="_blank"> study assessing value-based decision-making in Parkinson’s disease apathy  </a> (see <a  href="https://www.dundee.ac.uk/stories/parkinsons-patients-work-their-brains-harder-stay-motivated"  target="_blank"> here </a> for summary.

## Description
Intending to be a portable; OS/ device agnostic task.
Builds work for for webGL, iOS, Mac & Windows.

Original motivation was to deliver task on MR compatible tablet (iPad- see MRiOS branch).
See <a  href="https://gilbertson2209.github.io/thal-val/"> WebGL build</a> for a demo/ to play through.

## Getting Started
To build; download Unity (see dependencies); and import & build
C# code should be accessible and understandable; and as such doesn't fully leverage advanced features of the C# language.
Task in Main matches Daw's Restless Bandit params (see the supplementary material for details); but these are exposed in the inspector to allow anyone to play about with them.


### Dependencies
Unity 2022.3.16f1; 2D (URP)
(Packages in package list currently not clean and include packages from other projects)

### Builds & Installing
Follow Unity Build Processes but just ask if help needed; always happy.

### Logs, Data
Data read out to the [Unity Application Persistent Data Path](https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html); WebGl results are written onto the browser console. Looking to improve this in a low tech way (eg just read out something sensible to idbfs).
#### MRiOS & Windows builds: Emailing Responses
These branches have scripts that should email the data at the end of the task to the experimenter.
(The timing and play parameters may be altered in these branches compared to original).

To do this with minimal effort, open a new gmail account and add 2-factor auth. Once done, **SIGN IN TO THE NEW ACCOUNT then  head to** https://myaccount.google.com/apppasswords where you can create an app password. The Email Data (script) component attached to GameManager has empty fields in the inspector- fill with this new email and this 'app password' (not the gmail account password).

## Help
For code/ build [@i-brnrd](https://github.com/i-brnrd) on here or via [University of Dundee](https://www.dundee.ac.uk/people/isla-barnard).
For use of it try [here](https://www.dundee.ac.uk/people/tom-gilbertson) instead.

## Authors
Isla Barnard (developer)

Graham Mackenzie, Will Gilmour, Tom Gilbertson (lead)

## Acknowledgments
To [Nathanial Daw](https://dawlab.princeton.edu) for <a  href="10.1038/nature04766"  target="_blank">Daw's Four-Armed Restless Bandit</a>

## License
See [here](/LICENCE).
