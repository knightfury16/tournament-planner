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

## Requirement remod
 -> After all player has register for tournament. 
   - I want to **Draw** that is create and distribute players in groups. 
   - If a **Draw** has already been made and all the matches is not complete then will not allow to make the **Draw**. 
   - During the first request of **Draw**, admin can give a *list of players* that is **seeders**, who will not be in the same *group or evenly distributed among the groups*.
   -  After a draw is made **admin** can exchange players between Groups.
        - This exhange need be recorded.
   -  And then **Admin** can schedule match among the players in group.
        - All the matches will have round associated with it along with group and tournament.
   - After a match is played **Admin** can update the match with the played score and mark it as complete.
   - There need to be a service to get the standing of the players in Group.
   - players are ranked according to the standards rules. See the [Basic Group Ranking principle](#basic-priciple-of-group-ranking)
   - After all the match of groups are finished admin can request the next draw.
        -  That is the elimination round.
        -  By taking the players according to the rank from all the group.
   -  And the game is played in the elimination round as per elimination round. 


## Basic Priciple of Group Ranking

|   Player   |  A   |  B   |  C   |  D   | Win/Loss | Game Difference | Match Points | Ranking |
|------------|------|------|------|------|----------|-----------------|--------------|---------|
| A          | --   | 3-1  | 0-3  | 1-3  | 1/2      | 5-9             | 4           | 3       |
| B          | 3-1  | --   | 3-2  | 2-1  | 3/0      | 8-4             |             | 2       |
| C          | 0-3  | 2-3  | --   | 3-1  | 1/2      | 5-7             | 4            | 4       |
| D          | 3-1  | 1-2  | 1-3  | --   | 2/1      | 5-6             | 5            | 1       |


### First Criteria(Match Point)
- Overall result of each match
    1. Winner of a match is awarded **2 Points** whether it is played or a walkover/not played.(winning point can be set dynamically)
    2. Loser is awarded **1 Points** in played and *none* if match is not played.(Loser point can also be set dynamically, but the ratio should be **1:2**)
    3. Matches which are started but not complete will be treated as not played at all.

    Example (based on the table above):

    Player A: 1 win (vs B) = 2 point.
    Player B: 2 wins (vs C and D) = 4 points.
    Player C: 2 wins (vs A and D) = 4 points.
    Player D: 2 wins (vs A and B) = 4 points.

    Initial ranking: B, C, D, followed by A.

### Second Criteria (Hnadle Ties)
- **Head-to-head result**
    1. If two or more players have the samw match points, use **head-to-head** result to break the ties.
    2. Focus only on the matches played between the tied players.
    3. Rank based on the number of wins between those tied players.
- **Game Difference** or **Game Ratio**
    1. If **head-to-head** result are tied, calculate the game ration or difference for the *tied Players*
    2. Rank again by the game ratio or game difference.
- **Points Difference** or **Points Ratio**
    1. If players are still tied after using **head-to-head** and **game ratio**, calculate the **Points difference** or **Points ratio**
    2. Again rank players according to this **points ratio** or **points difference**

### Final Criteria
- If after applying all the tie breaker, there still persists tie, we may need to apply external rules like the following, 
    1. Random Draw
    2. Play off
    3. tossing a coin

### Some other scenerios
- Player injured/Match not completed
    1. If a player is injured or concede the match in mid of match, the match is marked as not played.
    2. The player who conceded will not receive the *1* points that a loser get of a played match
    3. The player who won will the the winning *2* points, whether match is played or not.
    4. If the match is unplayed, that is not even started then the winner player is awarded enough points to be declared as winner.
        - Enough points are **3-0** for match and for points **11-0**
    5. If a match is partylally played, that is match is abandoned in between the play, all points already scored are counted
        - If a player is injured and has to retire when *leading* **5-3** in the final game of best of *5 game*, the winner's score will be recorded as **11-5**.
        - Actual score of the previous game will remain unaltered.





