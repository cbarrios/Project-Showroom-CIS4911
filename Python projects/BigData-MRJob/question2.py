"""
 Question 2: Which are the top three cities that had the most total amount of gross revenue?
 Using Map-Reduce framework (mapper, combiner, and reducer functions) with mrjob package
 4/17/19
"""
import heapq
from mrjob.job import MRJob
from mrjob.step import MRStep

TOPN = 3

class TopThreeCityRev(MRJob):
# each input lines consists of city, productCategory, price, and paymentMode

    def mapper(self, _, line):
        # create a key-value pair with key: city and value: price
        line_cols = line.split(',')
        yield line_cols[0], float(line_cols[2])

    def combiner(self, city, counts):
        # consolidates all key-value pairs of mapper function (performed at mapper nodes)
        yield city, sum(counts)

    def reducer(self, city, counts):
        # final consolidation of key-value pairs at reducer nodes
        yield city, '${:,.2f}'.format(sum(counts))

    def topN_mapper(self, city, counts):
		# output: "Top 3" ["count", "city"]
        yield "Top "+str(TOPN), (counts, city)

    def topN_reducer(self, _, counts):
        for	count in heapq.nlargest(TOPN, counts):
            yield _, count

    def steps(self):
        return [ 
			MRStep(mapper=self.mapper, combiner=self.combiner, reducer=self.reducer), 
			MRStep(mapper=self.topN_mapper, reducer=self.topN_reducer)
		]

if __name__ == '__main__':
    TopThreeCityRev.run()