test_func :: (s: str, i: int, f: float, b: bool) -> void {
    print("Hi from test function in code.sail!");

    puts("\ns: ");
    print(s);

    puts("i: ");
    print(i);

    puts("f: ");
    print(f);

    puts("b: ");
    print(b);

    print("");
}

return_values :: (abc: int) -> int {
    return abc;
}

set_sail :: () -> str {
    print("\nDemo - Sail");
    print("----------------------------------------");

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

    add: float;

    // ** Maths is broken - no order of operations, uses dirty hacks to evaluate into int/float literals at parse time **
    // Also, reassignments currently use an xX_dirty-hack_Xx in the interpreter
    add := 50.2 - 290.1;

    puts("add: ");
    print(add);

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

    print("\nFunction call tests");
    print("-------------------");

    test_func("This is a string", 1_000_000, 12.301, true);

    print("Return values:");
    print("--------------");
    returnValue: int = return_values(2815);
    puts("returnValue: ");
    print(returnValue);

    print("\nIf statements:");
    print("--------------");    
    cond_1 := false;
    cond_2 := false;
    cond_3 := false;

    // With variables
    if cond_1 { 
        print("Condition 1 is true");   
    } elseif cond_2 {
        print("Condition 2 is true"); 
    } elseif cond_3 { 
        print("Condition 3 is true"); 
    } else { 
        print("EVERYTHING IS FALSE");
    }

    nested_func :: () -> str { return "Nested functions work :D\n"; }

    print("\nNested functions:");
    print("-----------------");
    print nested_func();

    print("For loops:");
    print("----------");

    goal := 10;
    for 1 .. goal {
        // "it" is an implicit variable which represents the current object of the iteration
        puts(it);

        if it < goal { puts(", "); }
    }

    print("\n\nComparison operators:");
    print("---------------------");

    someBool: bool = 10 <= 10;
    if someBool {
        print("someBool is true");
    } else {
        print("someBool is false");
    }

    return "\n\nSail works!";
}