# Hack-Challenge

## The Brief

The gauntlet has been thrown, and you have been challenged to create an web API that will enable users to play a game of [Tic-Tac-Toe](https://playtictactoe.org/) (or Noughts and Crosses - depending where you come from)

Your application will be a .NET Core application, and will use Kestrel as its internal web server.  When run, it should listen on `localhost` and a port of your choosing.

A number of HTTP routes will need to be handled, to enable a client to play and monitor games of Tic-tac-toe.

> Note:  In all of the HTTP Requests that are specified in the challenges below, if the request is successful and Http Code of 200 (OK) should be returned.

## Challenge 1

Create a .NET Core application that is setup to use Kestrel, and when run listens on `localhost` on port `8080`

### Creating a Tic-Tac-Toe game

Your application must handle an HTTP `POST` request to it on `http://localhost:8080/api/tttgame`, with json in the body that defines a new Tic-Tac-Toe game.

``` json
{
  "O" :  "Robin",
  "X" :  "Batman",
  "gameId" : "971b0a71-2569-4401-b722-32e13252b852"
}
```

If the game is successfully added, an http 200 (OK) response code is returned.

### List all the Tic-Tac-Toe games

It must also handle an HTTP `GET` request on the same URL of `http://localhost:8080/api/tttgame`, that should return an http 200 response (OK), with a  json array containing the `gameId`'s of ALL the games in the body of the response  E.g.

``` json
[ "971b0a71-2569-4401-b722-32e13252b852", "7f151bfe-1aef-45c5-947f-4529a89d809a","6902406e-b6c0-4a42-aa19-b50688b86ed9" ]
```

## Challenge 2

### Get full details of a single game

Your application must handle an HTTP `GET` request  to get details of a single game.  The URL will the same, but the `gameId` is added onto the path, so `http://localhost:8080/api/tttgame/[gameId]`. E.G

``` txt
http://localhost:8080/api/tttgame/6902406e-b6c0-4a42-aa19-b50688b86ed9
OR
http://localhost:8080/api/tttgame/971b0a71-2569-4401-b722-32e13252b852

```

This should return an http 200 response (OK) with the following json structure in body

``` json
{
  "O" :  "Deadpool",
  "X" :  "Thor",
  "gameId" : "7f151bfe-1aef-45c5-947f-4529a89d809a",
  "status" : "Win",
  "board" : [ "X", "O", "Empty", "X", "O", "Empty", "X", "Empty", "Empty" ],
  "winner" : "Deadpool"
}

OR

{
  "O" :  "Batman",
  "X" :  "Robin",
  "gameId" : "971b0a71-2569-4401-b722-32e13252b85",
  "status" : "Ongoing",
  "board" : [ "X", "O", "Empty", "X", "O", "Empty", "Empty", "Empty", "Empty" ],
  "winner" : ""
}
```

> The `status` field is a string, that will have one of 3 values.  `Ongoing`, `Draw` or `Win`.  If the status is `Win`, the `winner` field will contain the name of the winner.

The board is a 9 element array of strings, where the index positions of the array indicate the positions on the board as in the following

|     |     |     |
| --- | --- | --- |
| 0   | 1   | 2   |
| 3   | 4   | 5   |
| 6   | 7   | 8   |

so the board response from above of `[ "X", "O", "Empty", "X", "O", "Empty", "X", "Empty", "Empty" ]` represents the following board

|     |       |       |
| --- | :---: | ----- |
| X   | O     | Empty |
| X   | O     | Empty |
| X   | Empty | Empty |

### Make a play

To allow plays to be made on the board, you application should accept an HTTP `POST` request to it on `http://localhost:8080/api/tttgame/play`, with json in the body that defines a play. The format being

``` json
{
  "gameId" : "971b0a71-2569-4401-b722-32e13252b852",
  "player" : "X",
  "boardPosition" : 4
}
```

## Challenge 3

Correcly update the status of the game, when a play is made.

## Challenge 4

Manage exceptions and broken business rules.

If the any of the requests cannot be processed due to broken business rule, then an Http Status code of `400 - Bad Request` should be returned, with a message in body stating what went wrong. 

Examples of broken business rules are:

- creating a game with a `gameId` that already exists
- viewing or playing a game with a non-existant `gameId`
- playing in a board position that isn't empty, or doesn't exist

## Challenge 5

Persist the games across web application restarts

## Challenge 6

Ensure every http endpoint is async

## Challenge 7

Use dependency injection to inject in a different mechanism for storing the state of the games