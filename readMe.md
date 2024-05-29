Rectangles and Segments Intersection

Task:

- Implement a web api that would:
- Accept two pairs of doubles, representing a segment
- Search internal database and return a list of rectangles that interset the input segment by any of the edges

Summary:

For that task I created a .Net App with Api controller. For the task was chosen Hexagonal architecture, so also I added Business and Persistence projects.
For simplicity I used InMemoryStorage for development, but later added MongoDb which I run in docker container.
For Error handling implemented Result pattern which returns success or error objects.
Also I wrote 2 unit tests, where tested service and business logic class.

After all I Pushed it to Github.

To make setup easier for you I added a dockerfile and compose file. So when you checkout repo, you need to execute:

docker compose up

To test the api you have to open postman and send this request:

curl --location 'http://localhost:8080/findIntersectingRectangles' \
--header 'Content-Type: application/json' \
--data '[
{
"x": 0,
"y": 3
},
{
"x": 3,
"y": 3
}
]'

You should get 1 rectangle object as response, because in inmemorystorage i added 2 rectangles, one of them will be intersecting.
