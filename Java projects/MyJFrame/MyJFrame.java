package myjframe;

import javax.swing.JFrame;
import javax.swing.JMenuBar;
import javax.swing.JMenu;
import javax.swing.JMenuItem;
import javax.swing.JRadioButton;
import javax.swing.ButtonGroup;


public class MyJFrame extends JFrame
{
	JMenuBar menubar;
	JMenu f;
	JMenuItem mi;
	JRadioButton jrb;
	ButtonGroup bg;

	public MyJFrame(String title)
	{
		super(title);
		// The menubar is invisible
		menubar = new JMenuBar();
		setJMenuBar(menubar);
		bg = new ButtonGroup();

		buildMenu();
   	}
	void buildMenu()
	{
	    for (int i = 0; i < Constants.MENU.length; i++)
		{	//Build and add each menu choice onto the menubar
			f = new JMenu(Constants.MENU[i]);

		   switch(i)
		   {
		    	case 0: // Add menu items
		    		for (int j = 0; j < Constants.FILES.length; j++)
		    			if (Constants.FILES[j].equals("-"))
		    				f.addSeparator();
                                        else{
		    				f.add(mi = new JMenuItem(Constants.FILES[j]));
                                                mi.addActionListener(new Actions());
                                            }
		    	break;

		    	case 1:
		    		for (int k = 0; k < Constants.TOOL.length; k++)
		    			if (Constants.TOOL[k].equals("Edit"))
		    			{
		    				f.addSeparator();
		    				JMenu m = new JMenu(Constants.TOOL[k]); // Cascading menu
		    				for (int l = 0; l < Constants.EDIT.length; l++)
		    				{
		    					m.add(jrb = new JRadioButton(Constants.EDIT[l]));
		    					jrb.addActionListener(new Actions());
                                                        bg.add(jrb);
		    				}
		    				f.add(m);
		    			}
                                        else{
		    				f.add(mi = new JMenuItem(Constants.TOOL[k]));
                                                mi.addActionListener(new Actions());
                                            }
		    	break;
		     	case 2:
                                for (int h = 0; h < Constants.HELP.length; h++)
                                {
                                    f.add(mi = new JMenuItem(Constants.HELP[h]));
                                    mi.addActionListener(new Actions());
                                }
		    	
		    	break;
		       
		    }
		menubar.add(f);
		}
	}
        
       
}
