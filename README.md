# pinch

# About

A repo for Elevator code test.

# Requirements

You are in charge of writing software for an elevator (lift) company.
Your task is to write a program to control the travel of a lift for a 10 storey building.
A passenger can summon the lift to go up or down from any floor, once in the lift they can choose the floor they’d like to travel to.
Your program needs to plan the optimal set of instructions for the lift to travel, stop, and open its doors.

Some test cases:

    - Passenger summons lift on the ground floor. Once in, chooses to go to level 5.
    - Passenger summons lift on level 6 to go down. Passenger on level 4 summons the lift to go down. They both choose L1.
    - Passenger 1 summons lift to go up from L2. Passenger 2 summons lift to go down from L4. Passenger 1 chooses to go to L6. Passenger 2 chooses to go to Ground Floor
    - Passenger 1 summons lift to go up from Ground. They choose L5. Passenger 2 summons lift to go down from L4. Passenger 3 summons lift to go down from L10. Passengers 2 and 3 choose to travel to Ground.

You can implement this in any language you like (.NET preferred) and submit a working solution including tests and a readme explaining how to run it. Submissions on GitHub or BitBucket are preferred.

Please let me know if you have any questions.

# Assumptions

1) If elevator is more than 2 floors away then it's able to slow down and stop safely.
2) American or Australian system? Floor 1 is not Ground floor. Floor 0 is Ground Floor.

# Considerations

A) Safety requirements, eg: Acceleration and velocity likely have limits. Weight and passenger limits.
B) Wear and tear from over using one elevator. Should maybe spread the use of each elevator evenly.

# Thought Process

A) Identify main models.
B) Identify some basic actions for most basic requirement.
C) Implement basic requirement of just 1 elevator and 2 floors and confirm with unit test.
D) Expand requirements to work with 2 elevators.
E) Optimise system for better efficiency.

F) Ancillary services such as health checking, logging etc


# TODOs

Support for multiple elevators