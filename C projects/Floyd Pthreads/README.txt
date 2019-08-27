Description: HW4 - Multithreaded programming | Pthreads
Author: Carlos Barrios
Course: COP4520 - Spring 2019

--------How To Compile?--------
In order to compile the source code just type 'make' at the terminal with the provided Makefile.
**Note: User can type 'make clean' to delete the executable.

----How to run the executable?----
To properly execute the program 'floyd', please take a look at its usage:

-Usage   : ./floyd N INFILE ; where N = number of threads and INFILE = input file containing the adjacency matrix
-Example : ./floyd 4 mygraph

-Expected output for example above:
|||||||||||||||||||||||INPUT MATRIX|||||||||||||||||||||||
| 0 20 48 62
| 1024 0 1024 13
| 1024 1024 0 1024
| 7 16 1024 0
|||||||||||||||||||||||INPUT MATRIX|||||||||||||||||||||||

Floyd, matrix size 4, 4 threads:
|||||||||||||||||||||||OUTPUT MATRIX||||||||||||||||||||||
| 0 20 48 33
| 20 0 68 13
| 1024 1024 0 1024
| 7 16 55 0
|||||||||||||||||||||||OUTPUT MATRIX||||||||||||||||||||||