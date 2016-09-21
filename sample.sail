test_func :: (s: str, i: int, f: float, b: bool) -> void {
    print("Hi from test function!");

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

main :: () -> void {

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

    returnValue: int = return_values(2185);
    puts("returnValue: ");
    print(returnValue);
    
    cond_1 := false;
    cond_2 := false;
    cond_3 := false;

    if cond_1     { print("\nCondition is true");   }

    elseif cond_2 { print("\nCondition 2 is true"); }
    elseif cond_3 { print("\nCondition 3 is true"); }

    else          { print("\nEVERYTHING IS FALSE"); }

    if false    { print("\nLiteral condition is true");   }

    elseif true { print("\nLiteral condition 2 is true"); }
    elseif false { print("\nLiteral condition 3 is true"); }

    else          { print("\nEVERY LITERAL IS FALSE"); }

    nested_func :: () -> str { return "\nNested functions work :D\n"; }

    print(nested_func());

    goal := 10;
    for 1 to goal {
        // "it" is an implicit variable which represents the current object of the iteration
        print(it);
    }
}