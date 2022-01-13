# LobsterinkExercise

Before you start have a look at the sample decision tree used for this exercise.
```
        https://github.com/belwalkarmangesh/LobSterInk/blob/main/BookLover's_Dilema.png
```

Step 1: Once you have cloned the repo, if you want to run the unit tests to see if every thing is running follow below :
```
     a: docker run --name redis -p 6379:6379 -d redis => This will create redis container for testing.
     b: Run the Unit test from IDE.
```
Step 2: Once the test run successfully remove the redis container.
```
        docker rm -f redis

```

Step 3: From the root of project directory execute following command from command prompt.
```
        docker-compose -f docker-compose.yaml up
        
```

Step 4: Run below command, it should show the created images.
```
        docker images
```


Step 5: Run below command,  should list the created containers.
```
        docker container ls
```

Step 6: Open below URL in browser and you should be able to access the Api.
```
        http://localhost:5000/swagger

```
Actions in API :
```
       GetAllAdventures() : Get's all currently available adventure.
       For demo purpose we wil be only using Game 1 : Book lover's dilema.
```

```
       PlayGame(string gameName) : You can only play "Game 1" fro now , for starting game enter input "Game 1"
                                   Once the game is started only inputs allowed are "Yes" or "No"

```

``` 
       GetCurrentGameStepsTaken() : This action will list the steps taken for current game played

```
