# SphereManager
The application is part of a system that helps to organize the training process of athletes. General scheme of the system:
![alt text](https://user-images.githubusercontent.com/123956294/234098197-75ca8b1f-cce9-45e9-b101-38216f035880.jpg)

This application is missing from the diagram. It was added to control the balls that are used to mark the distance. The application reads messages from the RabbitMQ queue containing the coordinates of the athlete. According to the received coordinates, the application sends a signal to the balls to change their position or color. The application receives data about athletes from the server part of the system via HTTP REST in JSON format.
