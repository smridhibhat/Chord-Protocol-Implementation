# Project 3: Chord Protocol Implementation

# Team Members:
This project is completed by Smridhi Bhat, UFID: 26010147

COP5615: Project 3

# Build Process:

1. Download the zip file Project3.zip and unzip the folder.
2. Open command prompt or terminal, give command dotnet fsi Project3.fsx numberOfNodes numOfRequests.
In numberOfNodes, you can give the number of nodes(a positive integer value) for which the algorithm should run with. In noOfRequests, you can give the number of requests that the chord protocol has to traverse until the key is found. 

# What is Working :

We have implemented chord protocol using actor model in which each node/actor is assigned a finger table to store ids of other nodes in the network. This routing helps in finding the keys fast when compared to a client-server model.

In this project, we have used random number approach as the operation of hashing followed by m bit reductions was creating a lot of collisions while generating unique IDs.

# What is the largest network you managed to deal with

For this algorithm, the largest network that managed to deal with was 1000 nodes with 100 messages each. The range for the average number of hops was [9,11] for a message to reach the destination. The value came out to be 9.43 hops after averaging it over 10 hops. Hence, this means that for a single node to search for a particular key, it is taking an average of 9.43 for this protocol which shows that the lookup/search takes an order of LogN time.

The maximum number of nodes that can be experimented could be more than 1000 but it would take a lot of time to stabalize these nodes. This protocol runs better for higher number of messages.


# Interesting observation

We observed that as the number of keys are increased the value of average hops got decreased as the chances of finding a particular key increases.





