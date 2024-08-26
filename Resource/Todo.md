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
    - [ ] There will be **three** tab in the home screeen. Showing **Recent**,**This Week**(default),**Upcoming**
    - [ ] Show an option if the tournament is booked out or player can register for the tournament.
    - [ ] If not booked out then a valid **registered player** can **register** for the tournament going thorugh the necessary process.
    - [x] Sort it by createdDate field
    - [ ] User can search through the tournament with via the advance search option with the following params:
        - Tournament name
        - Start date and End date
        - Tournament Status 
        - Game Type (*eg. Table Tennis, chess*)
        - Search Category (*eg.Recent, Upcoming*)
- [ ] Create tournament
    - [ ] **Only Admin** can create tournament. If you are not an admin don't show the *Add Tournament* option.
    - [ ] Must requirement for a tournamnet are the following field:
        - Tournament Name
        - Start Date 
        - End Date
        - Game Type (*Table Tennis*)
        - Game Type (*Table Tennis*)
        - Tournament Status
    - [ ] Optional requirement for tournament creation:
        - Registration Last Date
        - MaxParticipant
        - Venue
        - Registration Fee
        - Minimum Age of Registration
        - Winner Per group
        - Knockout Start number
        - Participant Resolution Strategy
        - Tournament Type
    - [ ] (**Advande**) Once a tournament has been created, sent out an email notification to all the registered players interested in that game. 
    - [ ] (**Advande**) Maybe later, Add some categorization (on sports basis) to the tournament, and sent notification to users only interested in those particular  category of sports.

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

