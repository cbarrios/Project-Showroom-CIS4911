"""
 Question 3: How many distinct product categories are in the data file?
 Using Map-Reduce framework (mapper, combiner, and reducer functions) with mrjob package
 4/17/19
"""
from mrjob.job import MRJob
from mrjob.step import MRStep

class DistCategoriesCount(MRJob):
# each input lines consists of city, productCategory, price, and paymentMode

    def mapper(self, _, line):
        # create a key-value pair with key: productCategory and value: 0
        line_cols = line.split(',')
        yield line_cols[1], 0

    def combiner(self, category, counts):
        # consolidates all key-value pairs of mapper function (performed at mapper nodes)
        yield category, 0

    def reducer(self, category, counts):
        # final consolidation of key-value pairs at reducer nodes
        yield category, 1

    def mapper2(self, _, line):
        # create a key-value pair with key: 1 and value: 1
        yield line, 1

    def combiner2(self, category, counts):
        # consolidates all key-value pairs of mapper function (performed at mapper nodes)
        yield category, sum(counts)

    def reducer2(self, category, counts):
        # final consolidation of key-value pairs at reducer nodes
        yield "Distinct Product Categories", sum(counts)

    def steps(self):
        return [ 
			MRStep(mapper=self.mapper, combiner=self.combiner, reducer=self.reducer), 
			MRStep(mapper=self.mapper2, combiner=self.combiner2, reducer=self.reducer2)
		]

if __name__ == '__main__':
    DistCategoriesCount.run()