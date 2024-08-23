# Todo


- [ ] Write architecture test
    - use nuget NetArchTest.Rules
- [ ] Write Integration test
- [x] Write Unit test for all the useCases
- [x] Add automapper
- [ ] Add constrain on reschedule match
- [x] Do the exception properly
- [ ] Integrate Role
- [x] Migrate to CQRS
    - Use mediatr(created own implementation)
- [x] Refactore IRepository

## Change Requirements
- [x] Add createdAt DateTime property to all entity.[Domain Change] 


> [!NOTE]
> Keep the following points in mind

> [!CAUTION]  
>Keep in mind the type of object, **reference type** or **value type**, you are handling *(inserting in other places or updating)* in `js`. Think about the side effects!


### Client Requirements

## Tournaments

- [ ] List out all the tournaments
    - [x] List(default view) out all the tournament that is **ongoing** and are **scheduled** in the future from **now**
    - [ ] Show an option if the tournament is booked out or player can register for the tournament.
    - [ ] If not booked out then a valid **registered player** can **register** for the tournament going thorugh the necessary process.
    - [x] Sort it by createdDate field
    - [ ] Keep an option to show the **past** tournaments already happened. Maybe a toggle switch.
    - [ ] User can search through the tournament with name parameter and date all through the records that is **past** and **future** tournament.
- [ ] Create tournament
    - [ ] **Only Admin** can create tournament. If you are not an admin don't show the *Add Tournament* option.
    - [ ] Must requirement for a tournamnet are the following field:
     - Name
     - Start Date
     - End Date
     - Game type : Table Tennis, Chess, Football
    - [ ] During tournament creation **Name** field and **Start Date** is must required. Right now its configured only for **Name** field to be required.
    - [ ] Once a tournament has been created, sent out an email notification to all the registered players interested in that game. **Advance**
    - [ ] Maybe later, Add some categorization (on sports basis) to the tournament, and sent notification to users only interested in those particular  category of sports.

## Matches

 - [ ] Once a user click a tournament, take him to *Tournament Match* view.
    - [ ] List out the **latest** matches of tournament.
    - [ ] List out the matches on the basis of date.
    - [ ] Each match record will show match name, participants name, match points
    - [ ] User can filter through the matches by player name.
    - [ ] Maybe later, add an upvote option to show the round favourite and tournament favourite

- [ ] After a match has been played the admin of that match will be able to enter the match record/ That is save the record.
- [ ] Admin can reschedule a match.
- [ ] A player can request a reschedule of his/her match.
- [ ] Scheduling algorithm will be determined later, but the schedule will be made auto.
- [ ] Any viewer will be able to see matches and its result, as well as all the participant of that tournament.

