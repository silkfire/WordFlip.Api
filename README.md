# WordFlip - Wordsmith's premium word-flipping service
The WordFlip API has come to be one of Wordsmith's most popular services. We decided that it would be a pity if we didn't share the source code with the rest of the world and allowed anyone to enjoy the magic behind the service as much as we did making it.

## Features

* Reverses the individual words of any sentence, but preserves their original order.
* Retains the position of some leading and trailing punctuation marks that may surround the individual words.
* Persists the flipped sentences to any SQL Server database.

## How to use

The API comes with two methods, one to perform a word flip and persist the sentence to a database, and one to retrieve the last added sentences (the number of sentences fetched defaults to 5).

### Flip the words of a sentence

#### `POST /api/flip`

This method expects a JSON object in the following form:

```
{
   originalSentence: "I am a sentence ready to be flipped."
}
```

The payload must follow the specified JSON format and the input sentence cannot be `null` or empty. If everything goes well, you should receive a response containing the persisted FlippedSentence record:

```
{
   created: "\/Date(1534923486576)\/",
   id: 1
   sentence: "deppilf eb ot ydaer ecnetnes a ma I."
}
```

If the `originalSentence` property of the payload is missing, `null` or empty, you will receive the following response:

```
{
    "error": "'originalSentence' cannot be null or empty."
}
```


### Retrieve the last added sentences

#### `GET /api/flip/getLastSentences`

This method will fetch the last set of sentences added to the database ordered by their time of insertion in descending order.
The method returns an array of flipped sentence objects and follows the below JSON format:

```
[
   {  
      "id": 1,
      "sentence": "deppilf eb ot ydaer ecnetnes a ma I.",
      "created": "2018-07-01T14:55:00"
   }
]
```

### Error handling

If any exception occurs or you try to access a non-existing method, you will get the following default response:

```
{
   "error":"An unexpected error occurred."
}
```


## How to install

WordFlip works excellently if deployed to a set of Docker containers, one for the .NET backend and one for the database.
Regardless, you can still build it locally and adjust the connection string accordingly by altering the `appsettings.json` configuration file.

### Clone the repository

Clone the project into an empty directory:

```
$ git clone https://github.com/silkfire/WordFlip.Api.git
```

Run Docker Compose to build the necessary images:

```
$ cd WordFlip.Api
$ docker-compose build
```

Start the containers:

```
$ docker-compose up -d
```

You can now access the API from `localhost:8080`. This is the default port but can be changed in the Compose file `docker-compose.yml`.


To stop the service, run the following command:

```
$ docker-compose stop
```

To restart the service, run the following command:

```
$ docker-compose start
```

To delete the containers (in case you're rebuilding the project or just would like to reset the service), run the following command:

```
$ docker-compose down
```


### Happy flipping!

