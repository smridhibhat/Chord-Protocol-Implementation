#r "nuget: Akka"
#r "nuget: Akka.FSharp"

open System
open Akka.Actor
open Akka.FSharp

let numOfnodes = int (fsi.CommandLineArgs.[1] )
let numOfRequests = int (fsi.CommandLineArgs.[2])

//-------------------------------------------Initialization----------------------------------------------------------//
type ChordMesaagesTypes =
    
    | Search of int
    | SearchEnd
    | StartMaster
    | ForwardSearch of int * int
    | Initialize of int

// calculating the size of the finger table
let m = (Math.Log2 (numOfnodes |> double)) |> int
let system = ActorSystem.Create("System")

//-----------------------------------------Worker Actor------------------------------------------------------------//
let ChordActor (mailbox: Actor<_>) =

    let mutable chordId: int = 0
    let mutable predecessorNode: int = 0
    let mutable successorNode: int = 0
    let mutable fingerTable : int[] = [||]
    let mutable nextValidId = 0
    fingerTable <- Array.zeroCreate (m) 

    let rec loop () =
        actor {

            let! message = mailbox.Receive()
            let sender = mailbox.Sender()
            
            match message with

            | Initialize (id) ->
                chordId <- id
                //printfn "Actor Node %d---------" chordId
                

                for y in [ 0 .. (m-1)] do
        
                    nextValidId <- chordId + pown 2 y
                    if nextValidId > numOfnodes - 1 then nextValidId <- nextValidId - numOfnodes
                    fingerTable.[y] <-  nextValidId 
                    

                   
                //for i in 0..m-1 do
                  //  printfn "Value of Finger Table %d" fingerTable.[i]

                if chordId = 0 then
                    predecessorNode <- numOfnodes - 1
                else
                    predecessorNode <- (chordId - 1) % numOfnodes
                successorNode <- (chordId + 1) % numOfnodes
               
                   

                //printfn "Successor node is -> %d" successorNode
                //printfn "Predecessor node is -> %d" predecessorNode
                //printfn "----------------------------------------------------------"    

            | Search key ->
                let mutable check: Boolean = true
                let mutable x: int = 0
                
                if key = chordId then
                    check <- false
                    sender <! SearchEnd
                else
                    while x < m do
                        if fingerTable.[x] = key then
                            check <- false
                            sender <! SearchEnd
                        else if x < m - 1 then
                            if ((key < fingerTable.[x + 1] && key > fingerTable.[x]) || fingerTable.[x + 1] < fingerTable.[x]) then
                                check <- false
                                sender <! ForwardSearch(key, fingerTable.[x])
                        x <- x + 1   
                if check then
                    sender <! ForwardSearch(key, fingerTable.[m - 1]) 
                      
            | _ -> ()

            return! loop ()
        }
    loop () 
    
//-----------------------------------------Master Actor------------------------------------------------------------//             
let Master (mailbox: Actor<_>) =

    
    let mutable chordArray = [||]
    let mutable totalHops = 0
    let mutable totalSearches = 0
    chordArray <- Array.zeroCreate (numOfnodes) 

    let rec loop () =
        actor {

            let! message = mailbox.Receive()

            match message with
            | StartMaster ->
                printfn "Building chord network"
               

                for i = 0 to numOfnodes - 1 do
                    chordArray.[i] <- spawn system (sprintf "Local_%d" i) ChordActor
                    chordArray.[i] <! Initialize(i)  
                    //System.Threading.Thread.Sleep(1000) 
       
                let rand = Random()
                printfn "Running 1 request per second on each node/actor"

                for i = 0 to numOfRequests - 1 do
                    for j = 0 to numOfnodes - 1 do
                        let randomNum = rand.Next(numOfnodes)
                        chordArray.[j] <! Search(randomNum)
                    
            
            | SearchEnd ->
                totalSearches <- totalSearches + 1
                if totalSearches =  numOfnodes *  numOfRequests then
                    printfn "Total number of requests made : %d" totalSearches
                    printfn "Average hop count of all searches : %f" (float (totalHops) / float (totalSearches))
                    mailbox.Context.System.Terminate() |> ignore
             | ForwardSearch (key, node) ->
                totalHops <- totalHops + 1
                chordArray.[node] <! Search(key) 
                
            | _ -> ()

            return! loop ()
        }
    loop ()  

//-------------------------------------- Main Program --------------------------------------//       
let MasterActor = spawn system "Master" Master
MasterActor <! StartMaster

system.WhenTerminated.Wait()  
                
                     
                    
                
                      
                    
                    
                     
            
                
                
                 
                           
                       
                

                

                

                

                

           

            

               
                
           
                          

                        
                            
                                
                                

                        

                

                      
                    
                        

                
                

                

                
               
                
                

            





    
    







