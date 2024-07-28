# Desing Overhaul(PENDING ON DECISION)

Need Complete design overhaul.

Was doing the project for single player and fixed 32 player knockout match.

Now I want to do it in generic with lot of control.

# User Entity

There will two type of user entity:

1. Player:
    - This user will be the entity who can participate in tournament.
    - Need lot of information about this entity to do proper analysis or stat calculation. For Example: Age, Weight, TournamentParticipated, GamePlayed, GameWin etc. 

2. Admin:
 - This entity can set a tournament.
 - While setting the tournament admin can set the max number of participant.
 - While setting the tournament, this entity can choose what type of tournament this will be, for example,
 Gorup Stage or Knockout, if GroupStaged then have to select the **number of winner per group** who will proceede to next, and **number** from which knockout will start. If knockout than participant number has to be **power of 2**.
 - If suppose we get less registered player than the max, then depending on the Admin, can choose a resolution strategy. For example, in a knockout match of **16** we get **11** participant. Then obviously we have to start with **8**, but which **8**? Then we can say based on player stat, if stat are all same then randomly take the strongest **5** who will proceed to next, and taking remaining **6** we start a knockout match and **3** winner of those match will proceed to next with the **8** and the usal logic will take place from now. This is one policy that can be applied. As said it depends on the Admin decision. 
 - In group tournament type, group will be created based on the **number of winner per group** who will proceed to next and **knockout start** number. Default value for **number of winner per group** is **2** and knockout start is **8**.
 - In each group, each player will play a match with each one of the player in group at once, **Round Robin**.
 - So this grouping is important, we can not put possible two final candidate, that is stat is very strong, in same group.
 - So based on player/team stat group creation will be evenely distributed, where all group will have an evenly distribution of **some stat value** of player/team.

 - Each Game have their **format of score**. So there can not be a generic score format. We can implement a system where for each specific game we have specific template published.
 - For now I have decided to work on **Tabel Tennis** format. 
 - This format will be set based on the tournamnet game type that will be set by admin during tournament creation.




