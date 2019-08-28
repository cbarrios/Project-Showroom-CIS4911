package myjframe;

import java.awt.event.*;
import javax.swing.JOptionPane;


class Actions implements ActionListener
{	
	DisplayText dt;
        
	
	public void actionPerformed(ActionEvent e)
	{	
		if (e.getActionCommand().equalsIgnoreCase("New"))
		{
			dt = new DisplayText("New File", "");
                        
		}
		else if(e.getActionCommand().equalsIgnoreCase("Open"))
		{
			BasicFile f = new BasicFile("Open");
			dt = new DisplayText(f.getName(), f.getContent());
		}
                else if(e.getActionCommand().equalsIgnoreCase("Close"))
		{
                    JOptionPane.showMessageDialog(null, "Click OK to close the GUI program.", "Window Closing",JOptionPane.INFORMATION_MESSAGE);    
                    System.exit(0);
		}
                else if(e.getActionCommand().equalsIgnoreCase("Close My GUI"))
		{
                    JOptionPane.showMessageDialog(null, "Click OK to close the GUI program.", "Window Closing",JOptionPane.INFORMATION_MESSAGE);    
                    System.exit(0);
		}
		else if(e.getActionCommand().equalsIgnoreCase("Free Hand Drawing"))
		{
			new SimpleWhiteBoard();
			JOptionPane.showMessageDialog(null, "Now You Can Start Drawing Anything You Like!","Simple White Board",JOptionPane.INFORMATION_MESSAGE);
		}
                else if(e.getActionCommand().equalsIgnoreCase("My House"))
		{
			 new MyGraphics();
			 JOptionPane.showMessageDialog(null, "Now You Can Start Drawing The House You Like!","Draw Your House",JOptionPane.INFORMATION_MESSAGE);
		}
		else if(e.getActionCommand().equalsIgnoreCase("My Java Browser"))
		{
			 new Browser("My Java Web Browser");
			JOptionPane.showMessageDialog(null, "Now You Can Start Surfing The Web! Enjoy!","My Java Browser",JOptionPane.INFORMATION_MESSAGE);
		}
                else if(e.getActionCommand().equalsIgnoreCase("About"))
		{
			 JOptionPane.showMessageDialog(null,"Author: Carlos Barrios\nInstructor: Joslyn A. Smith\nCourse: COP3337\nDate: 4/28/2017"
                         ,"About My GUI", JOptionPane.INFORMATION_MESSAGE);
			
		}
		;

		
	}
}
