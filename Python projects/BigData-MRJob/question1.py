"""
 Question 1: What is the payment mode that had the highest total amount of gross revenue?
 Using Map-Reduce framework (mapper, combiner, and reducer functions) with mrjob package
 4/17/19
"""
from mrjob.job import MRJob
from mrjob.step import MRStep

class MaxModeRevenue(MRJob):
# each input lines consists of city, productCategory, price, and paymentMode

    def mapper(self, _, line):
        # create a key-value pair with key: paymentMode and value: price
        line_cols = line.split(',')
        yield line_cols[3], float(line_cols[2])

    def combiner(self, mode, counts):
        # consolidates all key-value pairs of mapper function (performed at mapper nodes)
        yield mode, sum(counts)

    def reducer(self, mode, counts):
        # final consolidation of key-value pairs at reducer nodes
        yield None, ('${:,.2f}'.format(sum(counts)),mode)
	
    def reducer_find_max(self, _, counts):
		# output: "Max Mode Revenue"     ["max", "mode"]
        yield "Max Mode Revenue", max(counts)

    def steps(self):
        return [ 
			MRStep(mapper=self.mapper, combiner=self.combiner, reducer=self.reducer), 
			MRStep(reducer=self.reducer_find_max)
		]
		
if __name__ == '__main__':
    MaxModeRevenue.run()
