# ASH Language
Repository for the ASH programming language.

# Introduction

The ASH Programming language was originally used for my own internal 
use for my ASHLI shell (A Terminal Emulator) and i finally released it
to do more than just command running. ASH stands for Ashli SHell.

# How to run code

You must creare a file with the .ash extention and load it into the .exe.

# Coding

ASH has only a few rules.

* 1 Each line of instruction has a command and a commandargs.
(1)PRINT (2): (3)Hello World

    * 1 The Command
    * 2 Separator 
    * 3 Commandargs - if the first word if the commandargs is a variable it will print the variable instead.

* 2 the language has locked values (constants) that cannot be written to once created
unless they are unlocked
Example:
```
SET : name Draven
LOCK : name
SET : name NotDraven
```

The second SET statement  will not execute because the variable cannot be set since its locked

But you can unlock all non language variables (language variables include ```pi```)

```
UNLOCK : name
```

# Current Language Keywords

* 1 ADD : var val - Add a var with value of val
* 2 SET : var val - Set a var with value of val
* 3 REM : var - Remove a var (work in progress)
* 4 PRINT : string | var - Print a string of chars or if its a variable it will print the variables
* 5 ##: - comment
* 6 ARITH : var expr - mathmatical expression +,-,/,*,% (Modulo)
* 7 READ : var - read input and store to var
* 8 EXIT : code - exit the progam with a code
* 9 GOTO : line - Goto line int execution (work in progress)
* 10 ARITH#SIN : var val - set var to value of sin(val)
* 11 ARITH#COS : var val - set var to value of cos(val)
* 12 ARITH#TAN : var val - set var to value of tan(val)
* 13 ARITH#SQRT : var val - set var to value of sqrt(val)
* 14 LOCK : var - make a var a constant
* 15 UNLOCK : var - unmake a var a constant
* 16 GOTO : location - goto the line in the file and continue execution from there
* 17 RAND : var - store a random number to a var

### Language constants and usable values

* 1 pi - constant equal to 3.1415
* 2 TIME - current time
* 3 CURLINE - current line
* 4 SYSTIME - miliseconds that have passed since execution'


# If you want to help

you can  contact me on twitter by navigaing to [InfiniteStrike - Contact] (http://infinitestrikeltd.github.io/contact)
