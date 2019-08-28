package myjframe;


import java.awt.*;
import java.awt.event.*;
import javax.swing.*;

public class SimpleWhiteBoard extends JFrame
{
	protected int lastX, lastY;

  	public SimpleWhiteBoard()
  	{
	  	super("Simple White Board");
	  	lastX=0;
	  	lastY=0;
	  	Container c = getContentPane();
	  	c.setBackground(Color.white);
	
        //1. Record the (x, y) position of the mouse
        addMouseListener(new PositionRecorder());
	//2.  Make drawings
        addMouseMotionListener(new LineDrawer());
	    setBounds(900, 550, 1920, 1080);
	    setVisible(true);
  	}

	protected void record(int x, int y)
	{
		lastX = x;
		lastY = y;
	}

  // Record position that mouse entered window or
  // where user pressed mouse button.

	private class PositionRecorder extends MouseAdapter
	{
		public void mouseEntered(MouseEvent e)
		{
		     record(e.getX(), e.getY());
		}

		public void mousePressed(MouseEvent e)
		{
		  record(e.getX(), e.getY());
		}
	}

  // As user drags mouse, connect subsequent positions
  // with short line segments.

	private class LineDrawer extends MouseMotionAdapter
	{
		public void mouseDragged(MouseEvent e)
		{
			int x = e.getX();
			int y = e.getY();

			Graphics g = getGraphics();
                        
			g.setColor(Color.RED);
	

	     	record(x, y);
            

            g.setFont( new Font("Century", Font.ITALIC, 40));
            g.drawString("•", x, y);

		}
	}
}