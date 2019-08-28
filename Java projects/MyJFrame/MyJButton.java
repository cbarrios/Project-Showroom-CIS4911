package myjframe;

import javax.swing.JButton;
import javax.swing.ImageIcon;
import java.awt.Container;
import java.awt.GridBagLayout;
import java.awt.GridBagConstraints;
import java.awt.Insets;


public class MyJButton extends JButton
{
	MyJFrame f;

	MyJButton(MyJFrame f)
	{
		this.f = f;
	}

	public void addButtons()
	{
	    JButton b;

		Container c = f.getContentPane();

		// Places components in a grid of rows & columns
		GridBagLayout gbag = new GridBagLayout();

		// Specify the constraints for each component
		GridBagConstraints constraints = new GridBagConstraints();

		c.setLayout(gbag); // Layout each component

		for (int i = 0; i < Constants.BUTTON.length; i++)
	    {
			b = new JButton(Constants.BUTTON[i]);
			switch(i)
			{
				case 0:
					// Specify the (x,y) coordinate for this component
					constraints.gridx = 0;
					constraints.gridy = 0; // (x,y) = (0,0)
 					constraints.insets = new Insets(25,0,25,0);
 				break;

				case 1:
					constraints.gridx = 0;
					constraints.gridy = 3;
                                        constraints.insets = new Insets(50,0,0,0);
				break;

				case 2:
					b = new JButton("My House",new ImageIcon(Constants.BUTTON[i]));// Add image
					constraints.gridx = 0;
					constraints.gridy = 1;
					constraints.insets = new Insets(25,0,25,0);
				break;
				case 3:
					constraints.gridx = 0;
					constraints.gridy = 2;
                                        
				break;
				}
			gbag.setConstraints(b, constraints);
                        c.add(b);
            
			b.addActionListener(new Actions());
		 }
 	}
}