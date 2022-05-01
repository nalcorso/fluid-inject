
### TL;DR

This project exists because I wanted to learn how to do it. It is inferior to existing DI solutions such as Autofac in every way. I would not (and do not) use 
this in production environements - again, there are better alternatives.

The design decisions are largely based on the type of API I wanted to learn about, rather than any real-world considerations.

### Why is this necessary?

### Container vs ContainerBuilder

### Fluent API Design vs Classic API Design

It is reasonable to choose either a Fluent API design (like Autofac) or a Classic API design (like Microsofts DI). In this project we will choose a Fluent API
for the simple (and slightly less _valid_) reason that I would like to learn how.

### Single Implementation vs Multiple Implementations

An argument can be made for restricting the container to only a single concrete implementation for a Type / Interface vs allowing multiple implementations.

#### Single Implementation

Pros:
 - Easier to reason about
 - Simpler Implementation of Type Resolution

Cons:
 - Harder to override behaviour at runtime

#### Multiple Implementations

Pros:
 - 

Cons:
 - 

For this project we will be allowing for Multiple Implementations.
