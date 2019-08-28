package myjframe;

import java.awt.Toolkit;
import java.awt.Dimension;
import java.awt.Rectangle;
import java.awt.Cursor;
import java.awt.Container;
import java.awt.Color;
import javax.swing.JFrame;
import java.awt.event.*;
import javax.swing.JOptionPane;

public class MyJFrame_TestClass {

    public static void main(String[] args) {
        // Get a JFrame object and initialize with title
        MyJFrame f = new MyJFrame(Constants.TITLE);
	
	// Get features of this frame
	Toolkit toolkit = f.getToolkit();
	
        // Get dimension of screen
	Dimension size = toolkit.getScreenSize();
	
        // Set properties for my frame
	Rectangle r = new Rectangle(Constants.X_POS, Constants.Y_POS, size.width/Constants.WIDTH, 2* size.height/Constants.HEIGHT);
	f.setBounds(r);
	
        // Get a Container object
	Container c = f.getContentPane();
	c.setBackground(Color.white);
	c.setCursor( Cursor.getPredefinedCursor(Cursor.WAIT_CURSOR ));
	
        // Adding the buttons
        MyJButton jb = new MyJButton(f);
        jb.addButtons();
        
        // Set operations for my frame
        f.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        f.setVisible(true);
        
        // Adding windowClosing event to the frame X(Close) button
        f.addWindowListener(new WindowAdapter(){
        public void windowClosing(WindowEvent e){
        JOptionPane.showMessageDialog(null, "Click OK to close the GUI program.", "Window Closing",JOptionPane.INFORMATION_MESSAGE);
        
        }
        });
        
	
    }
    
}
