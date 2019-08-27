Description: HW5 - Matrix Multiplication | Cannon Algorithm | MPI programming
Author: Carlos Barrios
Course: COP4520 - Spring 2019

--------How To Compile?--------
In order to compile the source code just type 'make' at the terminal with the provided Makefile.
**Note: User can type 'make clean' to delete the executable.

----How to run the executable?----
To properly execute the program 'cannon', please take a look at its usage:

-Usage   : mpirun -np p ./cannon fileA fileB fileC ; Input: fileA and fileB | Output: fileC
-Example : mpirun -np 1 ./cannon A B C

-Details: The program assumes 
(1)the matrices A, B, and C are n x n matrices. Ex: (1x1) or (2x2) or (3x3)...etc
(2)the number of processors p is square. 	Ex: 1 or 4 or 9 or 16 or 25...etc
(3)n is evenly divisible by sqrt(p).		Ex: n % sqrt(p) must be equal to 0
(4)matrices are stored in binary files (fileA fileB fileC) with specific format.
***Refer to the source code 'cannon.c' for full information.

-Expected output for the multiplication of two 4x4 matrices using 4 processors
-Assume we have A and B ready to read from:
A is the first input file containing the following matrix:
7.06 8.17 4.28 7.26
8.36 8.46 7.76 7.98
3.90 9.65 0.21 5.24
1.84 9.57 9.48 2.80

B is the second input file containing the following matrix:
9.41 1.22 6.81 7.05
0.72 7.19 0.05 0.64
9.09 6.53 6.82 7.76
4.92 5.10 0.31 7.12

-Example usage: mpirun -np 4 ./cannon A B C
id=0, coord[0,0]: read submatrix of A of dims 4x4
id=0, coord[0,0]: read submatrix of B of dims 4x4
id=1, coord[0,1]: read submatrix of A of dims 4x4
id=1, coord[0,1]: read submatrix of B of dims 4x4
id=2, coord[1,0]: read submatrix of A of dims 4x4
id=2, coord[1,0]: read submatrix of B of dims 4x4
id=3, coord[1,1]: read submatrix of A of dims 4x4
id=3, coord[1,1]: read submatrix of B of dims 4x4
elapsed time: 0.009275

-At this point we should have the resulting matrix in C:
146.89 132.37 79.96 139.94
194.51 162.42 112.81 181.44
71.27 102.23 30.11 72.64
124.12 147.25 78.58 112.67