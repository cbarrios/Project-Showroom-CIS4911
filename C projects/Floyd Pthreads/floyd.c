/*
 *   Floyd's all-pairs shortest path parallel algorithm using pthreads
 *
 *	 Requirement: Input graph must be generated randomly or manually with same structure
 *	 as it is generated with the program 'gengraph.c' provided by the instructor.
 *   
 *   Sample Input Matrix:
 *	 |    0   20   48   62 |
 *	 | 1024    0 1024   13 |
 *	 | 1024 1024    0 1024 |
 *	 |    7   16 1024    0 |
 *
 *	 Output Matrix Obtained:
 *   |    0   20  48   33  |
 *   |   20    0  68   13  |
 *   | 1024 1024   0 1024  |
 *   |    7   16  55    0  |
 */

#include <stdio.h>
#include <stdlib.h>
#include <limits.h>
#include <pthread.h>

//Macro to find the minimum element in a tuple
#define MIN(a,b)	((a)<(b)?(a):(b))

//Global declarations
int num_threads, num_nodes;
int** in_graph, **out_graph;
pthread_barrier_t barrier;
typedef int datatype;

//Floyd's all-pairs shortest path algorithm using pthreads
void* floyd(void* arg)
{
	long thread_id = (long)arg;
    int k,i,j;
	int** aux_graph;

    for(k = 0; k < num_nodes; k++)
	{
		for(i = thread_id; i < num_nodes; i+=num_threads) //row-wise partitioning
			for(j = 0; j < num_nodes; j++)
				in_graph[i][j] = MIN(out_graph[i][j], out_graph[i][k] + out_graph[k][j]);
        
		//Barrier: Sync all threads after work above is done to guarantee correct updates below
        pthread_barrier_wait(&barrier);
		
		//First child thread in charge of updates		
        if(thread_id == 0)
		{
            aux_graph = in_graph;	//most updated at kth iteration
            in_graph = out_graph;   //previous update
            out_graph = aux_graph;	//current update
        }
		
        //Barrier: All threads wait here until updates are done
        pthread_barrier_wait(&barrier);
    }
	
	//printf("Finished my floyd work ==> from child thread id '%d'\n", thread_id);
	return 0;
}

int main(int argc, char **argv)
{
	//Program must take exactly TWO command-line arguments 
	//(1)N = number of threads and (2)INFILE = name of the input file containing the adjacency matrix   
	if(argc != 3)
	{
		printf("Usage  : ./floyd N INFILE\n");
		printf("Example: ./floyd 4 mygraph\n");
		exit(1);
	}

	//Get number of threads
	num_threads = atoi(argv[1]);

	//Get matrix from input file
	int i,j,m,n;
	FILE* finptr;

	finptr = fopen (argv[2], "r");
	if(!finptr) 
	{
		perror("ERROR: can't open matrix file\n");
		exit(1);
	}
 
    if(fread(&m, sizeof(int), 1, finptr) != 1 || fread(&n, sizeof(int), 1, finptr) != 1) 
    {
		perror("ERROR: can't read matrix file\n");
		exit(1);
    }
	
	//Init global variable 'num_nodes'
	num_nodes = n;

	//Reserve space for global variable 'in_graph'
    in_graph = malloc(num_nodes * sizeof(int*));
    for (i = 0; i < num_nodes; ++i)
	{
        in_graph[i] = malloc(num_nodes * sizeof(int));
    }
	
	//Read from input file
	datatype *a;
	a = (datatype*)malloc(n*sizeof(datatype));
	for (i = 0; i < m ; i++) 
	{
		if(fread(a, sizeof(datatype), n, finptr) != n) 
		{
			perror("ERROR: can't read matrix file\n");
			exit(1);
		}
		for(j = 0; j < n ; j++)
			if(a[j] < INT_MAX)
			{ 
				in_graph[i][j] = (int)a[j]; //Populate input adjacency matrix
			}
    }
	free(a);
    fclose(finptr);
	
	//Display matrix from input file
	printf("|||||||||||||||||||||||INPUT MATRIX|||||||||||||||||||||||\n");
    for(i = 0; i < num_nodes; i++)
	{
        printf("| ");
        for(j = 0; j < num_nodes; j++)
		{
            printf("%d ", in_graph[i][j]);
        }
        printf("\n");
    }
    printf("|||||||||||||||||||||||INPUT MATRIX|||||||||||||||||||||||\n");
	
	//Reserve space for my duplicate matrix
    out_graph = malloc(num_nodes * sizeof(int*));
    for (i = 0; i < num_nodes; ++i)
	{
        out_graph[i] = malloc(num_nodes * sizeof(int));
    }
    //Make copy
    for(i = 0; i < num_nodes; i++)
	{
        for(j = 0; j < num_nodes; j++)
		{
            out_graph[i][j] = in_graph[i][j]; //Populate output adjacency matrix (duplicate of input matrix)
			
			//For sparse graphs. Fix 0s outside the diagonal by making them infinity
			if((out_graph[i][j] == 0) && (i != j))
			{
				out_graph[i][j] = INT_MAX;
				in_graph[i][j] = INT_MAX;
			}
        }
    }

    //Allocate space for threads
    pthread_t* thread;
	thread = (pthread_t*)malloc(num_threads*sizeof(pthread_t));

    //Barrier Init
    pthread_barrier_init(&barrier, NULL, num_threads);
        
    //Create threads
	long thread_id;
    for(thread_id = 0; thread_id < num_threads; thread_id++)
		pthread_create(&thread[thread_id], NULL, floyd, (void*)thread_id);

    //Join threads
    for(thread_id = 0; thread_id < num_threads; thread_id++)
		pthread_join(thread[thread_id], NULL);
 
	//Display result (output matrix)
	printf ("\nFloyd, matrix size %d, %d threads:",n, num_threads);
    printf("\n|||||||||||||||||||||||OUTPUT MATRIX||||||||||||||||||||||\n");
    for(i = 0; i < num_nodes; i++)
	{
        printf("| ");
        for(j = 0; j < num_nodes; j++)
		{
            printf("%d ", out_graph[i][j]);
        }
        printf("\n");
    }
    printf("|||||||||||||||||||||||OUTPUT MATRIX||||||||||||||||||||||\n");
    
	return 0 ;
}
