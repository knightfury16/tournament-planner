# Todo


- [ ] Write architecture test
    - use nuget NetArchTest.Rules
- [ ] Write Integration test
- [ ] Write Unit test for all the useCases
- [ ] Add automapper
- [ ] Add constrain on reschedule match
- [ ] Do the exception properly
- [ ] Integrate Role
- [ ] Migrate to CQRS
    - Use mediatr

## Change Requirements
- [x] Add createdAt DateTime property to all entity.[Domain Change] 


> [!NOTE]
> Keep the following points in mind

> [!CAUTION]  
>Keep in mind the type of object, **reference type** or **value type**, you are handling *(inserting in other places or updating)* in `js`. Think about the side effects!


### Client Requirements

- [ ] List out all the tournaments
    - [ ] List(default view) out all the tournament that is **ongoing** and are **scheduled** in the future from **now**
    - [ ] Show an option if the tournament is booked out or player can register for the tournament.
    - [ ] If not booked out then a valid registered player can register for the tournament going thorugh the necessary process.
    - [ ] Sort it by createdDate field
    - [ ] Keep an option to show the **past** tournaments already happened. Maybe a toggle switch.
    - [ ] User can search through the tournament with name parameter and date all through the records that is **past** and **future** tournament.
- [ ] Create tournament
    - [ ] **Only Admin** can create tournament. If you are not an admin don't show the *Add Tournament* option.
    - [ ] During tournament creation **Name** field and **Start Date** is must required. Right now its configured only for **Name** field to be required.
    - [ ] Once a tournament has been created, sent out an email notification to all the registered users/players. **Advance**
    - [ ] Maybe later, Add some categorization (on sports basis) to the tournament, and sent notification to users only interested in those particular  category of sports.
 - [ ] Once a user click a tournament, take him to *Tournament Match* view.
    - [ ] List out the matches of tournament on Round Basis.
    - [ ] *Default* show the user the latest round that is ongoing or going to play through.
    - [ ] For example if we are on half way on round 2 and then show the user directly the matches of round 2 and keep a separate tab where if user wants can see the matches of previous round.
    - [ ] Figure out a way to handle this requirement from backend.
    - [ ] User can filter through the matches by player name.
    - [ ] Maybe later, add an upvote option to show the round favourite and tournament favourite