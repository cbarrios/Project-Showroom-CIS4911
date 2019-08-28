(: 
  (Q1) For each branch, output:
    a) branch number and branch name
    b) total number of all book copies on hand in that branch
:)

<results>
  {
    for $branch in doc("../henry/Branch.xml")//Branch
    let $inventory := doc("../henry/Inventory.xml")//Inventory[BranchNum = $branch/BranchNum ]
    return
     <row>
       <Branch Number = "{$branch/BranchNum}"  Name = "{$branch/BranchName}" />
       <Stock BookCopies = "{sum($inventory/OnHand)}"/>
     </row>
   }
</results>
