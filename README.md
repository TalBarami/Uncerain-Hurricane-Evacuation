# Uncertain-Hurricane-Evacuation
## IAI191 assignment 4
### Goal:
Probabilistic reasoning using Bayes networks, with scenarios similar to the hurricane evacuation problem.

### Domain:
 As they try to find their best path, in the real world, evacuation forces may be unable to tell in advance what which road are blocked and where are there people to be evacuated, if any. There may be evidence which can help, but one cannot be sure until the node and or edge in question is reached! Not knowing the blockages and evacuee contents in advance makes it hard to plan an optimal path, so reasoning about the unknown is crucial. We know that it is more likely for people to need evacuation near blocked roads, and roads are more likely to be blocked if there is evidence of flooding at adjacent vertices. In this version of the problem, we will consider only binary-valued occupation of vertices, i.e. whether it contains people to be evacuated, or not.

Thus we have a binary random variable Fl(v) standing in for "flooding" at vertex v, one binary random variable Ev(v) standing in for "people to evacuate" at each vertex v, and a binary variable B(e) standing in for "blocked" for each edge e. The flooding events are assumed independent, with known distributions. The blockages are noisy-or distributed given the flooding at incident vertices, with pi =(1-qi)= 0.6*1/w(e). There are people at vertex v, with noisy or distributions given all the edge blockages at all edges incident on v, with pi=(1-qi)=0.8 for an edge with weight greater then 4, and with pi=(1-qi)=0.4 for shorter edges. All noisy-or node have a leakage probability of 0.001, that is, they are true with probability 0.001 when all the causes are inactive. The leakage may be ignored for the cases where any of the causes are active.

All in all, you have 3 types of variables (BN nodes): blockages (one for each edge) flooding (one for each vertex,) and evacuees present (one for each vertex). 

Input method is a file specifies the geometry (graph), and parameters such as P(Fl(v)=true)). Then, user can enter some locations where flooding, blockages, or evacuees are reported either present or absent (and the rest remain unknown). This is the evidence in the problem. Once evidence is instantiated, we perform reasoning about the likely locations of flooding, blockages, and evacuees (all probabilities below "given the evidence"):
1. What is the probability that each of the vertices contains evacuees?
2. What is the probability that each of the vertices is flooded?
3. What is the probability that each of the edges is blocked?
4. What is the probability that a certain path (set of edges) is free from blockages? (Note that the distributions of blockages in edges are NOT necessarily independent.)
5. What is the path from a given location to a goal that has the highest probability of being free from blockages?

User options:
1. Reset evidence list to empty.
2. Add piece of evidence to evidence list.
3. Do probabilistic reasoning (1, 2, 3, 4), or (1,2,3,4,5), whichever your program supports, and report the results.
4. Quit. 

```
#V 4          ; number of vertices n in graph (from 1 to n)

#V 1 F 0.2    ; Vertex 1, probability flooding 0.2
#V 2 F 0.4    ; Vertex 2, probability flooding 0.4
              ; Either assume flooding probability 0 by default,
              ; or make sure to specify this probability for all vertices.

#E1 1 2 W1 ; Edge1 between vertices 1 and 2, weight 1
#E2 2 3 W3 ; Edge2 between vertices 2 and 3, weight 3
#E3 3 4 W3 ; Edge3 between vertices 3 and 4, weight 3
#E4 2 4 W4 ; Edge4 between vertices 2 and 4, weight 4
```
