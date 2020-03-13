# PRCO204 Project - Supine
This repository is for the PRCO204 live client module for team Supine. We'll be using Unity for our PC game, web technologies (HTML/CSS/JS) for the tablet page and Node JS for the server between them.

Name|ID|Email|Roles|
---|---|---|---
Solomon Cammack|`10613265`|solomon@supine.dev|**Technical relations manager**, technical lead, repo manager, web manager
Alex Pritchard|`10577259`|alex@supine.dev|**Codebase manager**, technical lead, QA tester, Unity manager
Zack Hawkins|`10587295`|zack@supine.dev|**Client liason**, product owner, secondary designer
Josie Wood|`10509521`|josie@supine.dev|**Scrum master**, lead designer, QA tester, project management lead

# Version control structure
- All changes (other than documentation) will take place on their own branches.
- **Branch naming**: Branches will be named using lowercase **kebab-case** with a useful name for the contents of the branch. eg: `player-controls` `websocket-prototype` `level-generator` are good, `solomon-working-branch` `testing` `game` are not.
- Use your own Unity scene if you're working on distinct tasks. *You should ask yourself if you're the only one who's currently tasked with editing that scene* - if you're not sure, add a new scene and we can merge during our weekly sessions on Friday.
- **Branches require pull requests** before they're merged as a guide to the reviewer. Once you've published your branch (requires at least one commit), you can create a pull request on the [branches page](https://github.com/Plymouth-University/prco204-supine/branches). Keep these updated if major things change or if you require help. Use the features of GitHub to aid you - request reviews when you're done and mention other users for help.
- **Commit naming**: Please start your commit with the Jira issue you're working on in square brackets. Realistically, a commit should only address one issue. Good example: `[SUP-11] Fixed position of enemy mesh in prefab`.
- **Commit often**: When you've hit a milestone or big objective in your task, commit with a good, succinct message about what you've done.
- **Update [Jira](https://jira.slmn.io/projects/SUP)**: Longer form discussion & updates, especially when you've committed or have finished a sub-task should be written up on Jira. The more documentation, the easier it is for someone to take over. I would honestly prefer more detail on Jira than on commit descriptions, especially if you use the `[SUP-X]` code in your commit message.
