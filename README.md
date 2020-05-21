# PRCO204 Project - Supine
This repository is for the PRCO204 live client module for team Supine. We'll be using Unity for our PC game, web technologies (HTML/CSS/JS) for the tablet page and Node JS for the server between them.

Name|ID|Email|Roles|
---|---|---|---
Solomon Cammack|`10613265`|solomon@supine.dev|**Technical Lead**, technical relations manager, repo manager, web manager
Alex Pritchard|`10577259`|alex@supine.dev|**Technical Lead**, codebase manager, QA tester, Unity manager
Zack Hawkins|`10587295`|zack@supine.dev|**Product Owner**, client liason, secondary designer
Josie Wood|`10509521`|josie@supine.dev|**Scrum Master**, lead designer, QA tester, project management lead


- [YouTube playlist](https://www.youtube.com/playlist?list=PL6ksOWkFD7fa1cVO3ClEAlgyDCnsfLo5C) of our demonstrations
- Join the game in a browser: [play.supine.dev](https://play.supine.dev)

![Summary solution fact sheet](https://media.supine.dev/summary_solution_fact_sheet.png)

![Screenshot of the main menu](https://media.supine.dev/screenshots/Screenshot_139.png)
![Screenshot of the starting room](https://media.supine.dev/screenshots/Screenshot_140.png)
![Screenshot of combat](https://media.supine.dev/screenshots/Screenshot_141.png)

----

# Team meeting schedules
Due|Action|By
---|---|---
Saturday Afternoon (2pm start) | **Sprint review**: Discuss how the week's sprint went, and decide on tasks for the following weeks' sprint.|Everyone
Monday Afternoon (2pm start)|**Client meeting**: Discussing the project with the client, making sure we are staying on track and within scope. This is not every monday. |Everyone & Client
Tuesday Morning (12pm start)|**Live coding**: Working together in a discord call to develop the project, sharing screens if necessary.|Everyone
Wednesday Morning (12pm start)|**Live coding**: Working together in a discord call to develop the project, sharing screens if necessary.|Everyone
Thursday Evening (7pm start)|**Live coding**: Working together in a discord call to develop the project, sharing screens if necessary|Everyone

# Version control schedule
Due|Action|By
---|---|---
Friday at meeting|**Branch pull request**: Make sure you've made your pull request by or during Friday's meeting so it can be reviewed.|Branch developer
Friday/Saturday evening|**Initial branch reviews**: A technical lead will do the initial round of reviews by delving into your branch files and looking for things to change.|Technical leads
Monday|**Pull requests close**: Your review should be completed and you'll be able to see if there's anything that needs to be changed. These changes are usually small (like code style / obvious errors or improvements / quick fixes) and shouldn't take too long|Branch developer
Monday|**Pull requests merge**: A technical lead will close and merge your branch into master if it's ready.|Technical leads

# Version control structure
- All changes (other than documentation) will take place on their own branches.
- **Branch naming**: Branches will be named using lowercase **kebab-case** with a useful name for the contents of the branch. eg: `player-controls` `websocket-prototype` `level-generator` are good, `solomon-working-branch` `testing` `game` are not.
- Use your own Unity scene if you're working on distinct tasks. *You should ask yourself if you're the only one who's currently tasked with editing that scene* - if you're not sure, add a new scene and we can merge during our weekly sessions on Friday.
- **Branches require pull requests** before they're merged as a guide to the reviewer. Once you've published your branch (requires at least one commit), you can create a pull request on the [branches page](https://github.com/Plymouth-University/prco204-supine/branches). Keep these updated if major things change or if you require help. Use the features of GitHub to aid you - request reviews when you're done and mention other users for help.
- **Commit naming**: Please start your commit with the Jira issue you're working on in square brackets. Realistically, a commit should only address one issue. Good example: `[SUP-11] Fixed position of enemy mesh in prefab`.
- **Commit often**: When you've hit a milestone or big objective in your task, commit with a good, succinct message about what you've done.
- **Update [Jira](https://jira.slmn.io/projects/SUP)**: Longer form discussion & updates, especially when you've committed or have finished a sub-task should be written up on Jira. The more documentation, the easier it is for someone to take over. I would honestly prefer more detail on Jira than on commit descriptions, especially if you use the `[SUP-X]` code in your commit message.

# Code base structure
We will be following Microsoft's .NET framework [coding conventions](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions).

- **Comments**: 
Comments will be present throughout scripts, especially before a method detailing what that method does (and if so, what value it returns). Place the comment on a separate line, not at the end of a line of code. Comments must follow standardised English with appropriate punctuation.Insert one space between the comment delimiter (//) and the comment text. 

```
  // This is a proper comment. It does
  
  // not violate any rules.
  ```
  
- **Code**: Lines must be <75 characters in length. Write only one statement per line. Write only one declaration per line. If continuation lines are not indented automatically, indent them one tab stop (four spaces). Add at least one blank line between method definitions and property definitions.
- **IF Statements**: Do not use the operators != or == where appropriate, instead use (boolean) and (!boolean). Instead of using nested IF statements for checking for null values, use the ??, ?= operators.
- **Peer review**: Individual scripts should be checked for these standards before being merged into the master branch.

# .gitignore fixing
Sometimes the .gitignore isn't followed properly, especially across different clients. If you end up committing your Library directory or anything else that violates the .gitignore, you can run these commands in the root of the git folder to clean it up.

- First, commit anything that you've done.
- `git rm -r --cached .` (removes everything cached from the git)
- `git add .` (re-adds everything, passing it through the .gitignore)
