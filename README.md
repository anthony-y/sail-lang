# Sail

Sail is a toy programming language made for fun. It isn't finished yet.

## Can you actually run Sail code at the moment?

Yes.

## Should you use it?

No.

## Why not?

1. It's had just over 2 weeks of development time as I type this (19th Sep 2016)
2. The syntax is not yet concrete. I'm still deciding if I like certain elements of it. It may change.
3. Because there's loads of stuff missing.
4. It's a toy langauge made for fun and to learn.

## What's missing?

- while loops
- order of operations in maths expressions
- scope
- user defined types
- defer statement
- user input
- standard library
- type of
- string formatting

Yeah. Most of the language is missing :D

## Code example (syntax not concrete)

```go

someFunction :: (someNumber: int) -> int {
    print("Hi from someFunction");
    puts("Your number is ");
    puts(someNumber);

    return someNumber;
}

main :: () -> void {

    // Explicit type variable declarartion
    hello: str = "Hello, ";
    puts(hello);

    // Eliminate the type for it to be inferred from the right
    world := "World!\n";
    puts(world);

    // Currently has 4 types (including str)
    bools  : bool  = true; // or false
    ints   : int   = 144;
    floats : float = 144.44;

    print("\nExplicitly declared variables!");
    print("------------------------------");

    puts("bools: ");
    print(bools);

    puts("ints: ");
    print(ints);

    puts("floats: ");
    print(floats);
  
    // And of course type inference works on all these too
    inferBool  := false;
    inferInt   := 123;
    inferFloat := 123.4;

    print("\nInferred variables!");
    print("-------------------");

    puts("inferBool: ");
    print(inferBool);

    puts("inferInt: ");
    print(inferInt);

    puts("inferFloat: ");
    print(inferFloat);

    cond := true;
    if cond {
        print("\nCondition is true");
    } else {
        print("\nCondition is false");
    }

    nested_func :: () -> str { return "\nNested functions work :D\n"; }

    print nested_func();

    goal := 10;
    for 1 to goal {
        // "it" is an implicit variable which represents the current object of the iteration
        print(it);
    }

    myNumber: int = someNumber(144);
}
```

## Is Sail an acronym?

Yes.

## What does it stand for?

Not telling, too cringey.