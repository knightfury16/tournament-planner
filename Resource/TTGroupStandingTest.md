# Game Example

| Player | P   | Q   | R   | S   | T   | U   | Win/Loss | Game Difference | Points Difference | Match Points | Initial Rank |
|--------|-----|-----|-----|-----|-----|-----|----------|-----------------|-------------------|--------------|--------------|
| P      | --  | 3-2 | 2-3 | 3-1 | 0-3 | 3-0 | 3/2      | 11-9 = +3       | 196/191 = +5      | 8            |              |
| Q      | 2-3 | --  | 3-0 | 2-3 | 3-1 | 1-3 | 2/3      | 11-10 = +1      | 206/201 = +5      | 7            |              |
| R      | 3-2 | 0-3 | --  | 3-1 | 2-3 | 3-0 | 3/2      | 11-9 = +3       | 198/189 = +9      | 8            |              |
| S      | 1-3 | 3-2 | 1-3 | --  | 3-0 | 2-3 | 2/3      | 10-11 = -1      | 201/206 = -5      | 7            |              |
| T      | 3-0 | 1-3 | 3-2 | 0-3 | --  | 2-3 | 2/3      | 9-11 = -2       | 190/197 = -7      | 7            |              |
| U      | 0-3 | 3-1 | 0-3 | 3-2 | 3-2 | --  | 3/2      | 9-11 = -2       | 190/197 = -7      | 8            |              |

## Game sort by match points

| Player | P   | Q   | R   | S   | T   | U   | Win/Loss | Game Difference | Points Difference | Match Points | Initial Rank |
|--------|-----|-----|-----|-----|-----|-----|----------|-----------------|-------------------|--------------|--------------|
| P      | --  | 3-2 | 2-3 | 3-1 | 0-3 | 3-0 | 3/2      | 11-9 = +3       | 196/191 = +5      | 8            |              |
| U      | 0-3 | 3-1 | 0-3 | 3-2 | 3-2 | --  | 3/2      | 9-11 = -2       | 190/197 = -7      | 8            |              |
| R      | 3-2 | 0-3 | --  | 3-1 | 2-3 | 3-0 | 3/2      | 11-9 = +3       | 198/189 = +9      | 8            |              |

| S      | 1-3 | 3-2 | 1-3 | --  | 3-0 | 2-3 | 2/3      | 10-11 = -1      | 201/206 = -5      | 7            |              |
| T      | 3-0 | 1-3 | 3-2 | 0-3 | --  | 2-3 | 2/3      | 9-11 = -2       | 190/197 = -7      | 7            |              |
| Q      | 2-3 | --  | 3-0 | 2-3 | 3-1 | 1-3 | 2/3      | 11-10 = +1      | 206/201 = +5      | 7            |              |

## Game sort by game difference

| Player | P   | Q   | R   | S   | T   | U   | Win/Loss | Game Difference | Points Difference | Match Points | Initial Rank |
|--------|-----|-----|-----|-----|-----|-----|----------|-----------------|-------------------|--------------|--------------|
| P      | --  | 3-2 | 2-3 | 3-1 | 0-3 | 3-0 | 3/2      | 11-9 = +3       | 196/191 = +5      | 8            |              |
| R      | 3-2 | 0-3 | --  | 3-1 | 2-3 | 3-0 | 3/2      | 11-9 = +3       | 198/189 = +9      | 8            |              |

| U      | 0-3 | 3-1 | 0-3 | 3-2 | 3-2 | --  | 3/2      | 9-11 = -2       | 190/197 = -7      | 8            |              |

| Q      | 2-3 | --  | 3-0 | 2-3 | 3-1 | 1-3 | 2/3      | 11-10 = +1      | 206/201 = +5      | 7            |              |
| S      | 1-3 | 3-2 | 1-3 | --  | 3-0 | 2-3 | 2/3      | 10-11 = -1      | 201/206 = -5      | 7            |              |
| T      | 3-0 | 1-3 | 3-2 | 0-3 | --  | 2-3 | 2/3      | 9-11 = -2       | 190/197 = -7      | 7            |              |


## Game sort by points difference

| Player | P   | Q   | R   | S   | T   | U   | Win/Loss | Game Difference | Points Difference | Match Points | Initial Rank |
|--------|-----|-----|-----|-----|-----|-----|----------|-----------------|-------------------|--------------|--------------|
| R      | 3-2 | 0-3 | --  | 3-1 | 2-3 | 3-0 | 3/2      | 11-9 = +3       | 198/189 = +9      | 8            |              |
| P      | --  | 3-2 | 2-3 | 3-1 | 0-3 | 3-0 | 3/2      | 11-9 = +3       | 196/191 = +5      | 8            |              |

| U      | 0-3 | 3-1 | 0-3 | 3-2 | 3-2 | --  | 3/2      | 9-11 = -2       | 190/197 = -7      | 8            |              |

| Q      | 2-3 | --  | 3-0 | 2-3 | 3-1 | 1-3 | 2/3      | 11-10 = +1      | 206/201 = +5      | 7            |              |
| S      | 1-3 | 3-2 | 1-3 | --  | 3-0 | 2-3 | 2/3      | 10-11 = -1      | 201/206 = -5      | 7            |              |
| T      | 3-0 | 1-3 | 3-2 | 0-3 | --  | 2-3 | 2/3      | 9-11 = -2       | 190/197 = -7      | 7            |              |


## Game data

// P vs Q (3-2)
{
    "gamePlayed": "2024-07-09T16:02:07.401840Z",
    "gameSpecificScore": {
      "player1Sets": 3,
      "player2Sets": 2,
      "setScores": [
        { "player1Points": 11, "player2Points": 9 },
        { "player1Points": 9, "player2Points": 11 },
        { "player1Points": 11, "player2Points": 8 },
        { "player1Points": 8, "player2Points": 11 },
        { "player1Points": 11, "player2Points": 9 }
      ]
    }
}

-------------------------------------

// P vs R (2-3)
{
    "gamePlayed": "2024-07-09T16:02:07.401840Z",
    "gameSpecificScore": {
      "player1Sets": 2,
      "player2Sets": 3,
      "setScores": [
        { "player1Points": 11, "player2Points": 9 },
        { "player1Points": 9, "player2Points": 11 },
        { "player1Points": 11, "player2Points": 9 },
        { "player1Points": 7, "player2Points": 11 },
        { "player1Points": 9, "player2Points": 11 }
      ]
    }
}

-------------------------------------

// P vs S (3-1)
{
    "gamePlayed": "2024-07-09T16:02:07.401840Z",
    "gameSpecificScore": {
      "player1Sets": 3,
      "player2Sets": 1,
      "setScores": [
        { "player1Points": 11, "player2Points": 8 },
        { "player1Points": 11, "player2Points": 9 },
        { "player1Points": 9, "player2Points": 11 },
        { "player1Points": 11, "player2Points": 7 }
      ]
    }
}

-------------------------------------

// P vs T (0-3)
{
    "gamePlayed": "2024-07-09T16:02:07.401840Z",
    "gameSpecificScore": {
      "player1Sets": 0,
      "player2Sets": 3,
      "setScores": [
        { "player1Points": 9, "player2Points": 11 },
        { "player1Points": 8, "player2Points": 11 },
        { "player1Points": 7, "player2Points": 11 }
      ]
    }
}

-------------------------------------

// P vs U (3-0)
{
    "gamePlayed": "2024-07-09T16:02:07.401840Z",
    "gameSpecificScore": {
      "player1Sets": 3,
      "player2Sets": 0,
      "setScores": [
        { "player1Points": 11, "player2Points": 7 },
        { "player1Points": 11, "player2Points": 9 },
        { "player1Points": 11, "player2Points": 8 }
      ]
    }
}

-------------------------------------

// Q vs R (3-0)
{
    "gamePlayed": "2024-07-09T16:02:07.401840Z",
    "gameSpecificScore": {
      "player1Sets": 3,
      "player2Sets": 0,
      "setScores": [
        { "player1Points": 11, "player2Points": 8 },
        { "player1Points": 11, "player2Points": 9 },
        { "player1Points": 11, "player2Points": 7 }
      ]
    }
}

-------------------------------------

// Q vs S (2-3)
{
    "gamePlayed": "2024-07-09T16:02:07.401840Z",
    "gameSpecificScore": {
      "player1Sets": 2,
      "player2Sets": 3,
      "setScores": [
        { "player1Points": 11, "player2Points": 9 },
        { "player1Points": 9, "player2Points": 11 },
        { "player1Points": 11, "player2Points": 8 },
        { "player1Points": 8, "player2Points": 11 },
        { "player1Points": 9, "player2Points": 11 }
      ]
    }
}

-------------------------------------

// Q vs T (3-1)
{
    "gamePlayed": "2024-07-09T16:02:07.401840Z",
    "gameSpecificScore": {
      "player1Sets": 3,
      "player2Sets": 1,
      "setScores": [
        { "player1Points": 11, "player2Points": 8 },
        { "player1Points": 11, "player2Points": 9 },
        { "player1Points": 9, "player2Points": 11 },
        { "player1Points": 11, "player2Points": 7 }
      ]
    }
}

-------------------------------------

// Q vs U (1-3)
{
    "gamePlayed": "2024-07-09T16:02:07.401840Z",
    "gameSpecificScore": {
      "player1Sets": 1,
      "player2Sets": 3,
      "setScores": [
        { "player1Points": 11, "player2Points": 9 },
        { "player1Points": 8, "player2Points": 11 },
        { "player1Points": 9, "player2Points": 11 },
        { "player1Points": 7, "player2Points": 11 }
      ]
    }
}

-------------------------------------

// R vs S (3-1)
{
    "gamePlayed": "2024-07-09T16:02:07.401840Z",
    "gameSpecificScore": {
      "player1Sets": 3,
      "player2Sets": 1,
      "setScores": [
        { "player1Points": 11, "player2Points": 8 },
        { "player1Points": 11, "player2Points": 9 },
        { "player1Points": 9, "player2Points": 11 },
        { "player1Points": 11, "player2Points": 7 }
      ]
    }
}

-------------------------------------

// R vs T (2-3)
{
    "gamePlayed": "2024-07-09T16:02:07.401840Z",
    "gameSpecificScore": {
      "player1Sets": 2,
      "player2Sets": 3,
      "setScores": [
        { "player1Points": 11, "player2Points": 9 },
        { "player1Points": 9, "player2Points": 11 },
        { "player1Points": 11, "player2Points": 8 },
        { "player1Points": 8, "player2Points": 11 },
        { "player1Points": 9, "player2Points": 11 }
      ]
    }
}

-------------------------------------

// R vs U (3-0)
{
    "gamePlayed": "2024-07-09T16:02:07.401840Z",
    "gameSpecificScore": {
      "player1Sets": 3,
      "player2Sets": 0,
      "setScores": [
        { "player1Points": 11, "player2Points": 7 },
        { "player1Points": 11, "player2Points": 9 },
        { "player1Points": 11, "player2Points": 8 }
      ]
    }
}

-------------------------------------

// S vs T (3-0)
{
    "gamePlayed": "2024-07-09T16:02:07.401840Z",
    "gameSpecificScore": {
      "player1Sets": 3,
      "player2Sets": 0,
      "setScores": [
        { "player1Points": 11, "player2Points": 8 },
        { "player1Points": 11, "player2Points": 9 },
        { "player1Points": 11, "player2Points": 7 }
      ]
    }
}

-------------------------------------

// S vs U (2-3)
{
    "gamePlayed": "2024-07-09T16:02:07.401840Z",
    "gameSpecificScore": {
      "player1Sets": 2,
      "player2Sets": 3,
      "setScores": [
        { "player1Points": 11, "player2Points": 9 },
        { "player1Points": 9, "player2Points": 11 },
        { "player1Points": 11, "player2Points": 8 },
        { "player1Points": 8, "player2Points": 11 },
        { "player1Points": 9, "player2Points": 11 }
      ]
    }
}

-------------------------------------

// T vs U (2-3)
{
    "gamePlayed": "2024-07-09T16:02:07.401840Z",
    "gameSpecificScore": {
      "player1Sets": 2,
      "player2Sets": 3,
      "setScores": [
        { "player1Points": 11, "player2Points": 9 },
        { "player1Points": 9, "player2Points": 11 },
        { "player1Points": 11, "player2Points": 8 },
        { "player1Points": 8, "player2Points": 11 },
        { "player1Points": 9, "player2Points": 11 }
      ]
    }
}

