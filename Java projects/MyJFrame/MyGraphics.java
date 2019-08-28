package myjframe;

import java.awt.Font;
import java.awt.Color;
import java.awt.Container;
import java.awt.Graphics;
import java.awt.Polygon;
import javax.swing.JFrame;
import java.awt.event.*;


public class MyGraphics extends JFrame {
    
    protected int lastX, lastY;
    
    public MyGraphics(){
    super("My House - Make It Yours!");
    lastX=0;lastY=0;
    
    Container c = getContentPane();
    c.setBackground(Color.white);
    
    addMouseListener(new PositionRecorder());
    addMouseMotionListener(new LineDrawer());
    
    setBounds(900, 550, 1920, 1080);
    setVisible(true);
    }
    
    protected void record(int x, int y)
	{
		lastX = x;
		lastY = y;
	}
    
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
    
    private class LineDrawer extends MouseMotionAdapter
	{
		public void mouseDragged(MouseEvent e)
		{
			int x = e.getX();
			int y = e.getY();

			Graphics g = getGraphics();
                        
			g.setColor(Color.black);
		

	     	record(x, y);
            

            g.setFont( new Font("Century", Font.ITALIC, 40));
            g.drawString("•", x, y);

		}
	}
    
    public void paint(Graphics g){
        // Define yellow star
        Polygon p = new Polygon(Constants.X,Constants.Y,Constants.Y.length);
        g.setColor(Color.YELLOW);
        g.fillPolygon(p);
        
        // Draw the roof
	g.setColor(Color.red);
	int xs[] = {200,260,320};
	int ys[] = {400,350,400};
	Polygon poly=new Polygon(xs,ys,3);
	g.fillPolygon(poly);
		
	// Draw the body of house
	g.setColor(Color.blue);
	g.fillRect(200,400,120,120);
		
	// Draw the door
	g.setColor(Color.white);
	g.fillRect(245,460,30,60);
		
	// Draw chimney
	g.setColor(Color.black);
	g.fillRect(275,345,10,25);

        // Draw the string
        g.setFont( new Font("Century", Font.ITALIC, 40));
        g.drawString("My House", 170, 580);
        
        
    }
       
        
        
    
    }
        

