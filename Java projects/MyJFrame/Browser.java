package myjframe;

import javax.swing.JFrame;
import javax.swing.JScrollPane;
import javax.swing.JPanel;
import javax.swing.JTextField;
import javax.swing.JButton;
import javax.swing.JEditorPane;
import javax.swing.JOptionPane;
import javax.swing.event.*;
import java.awt.BorderLayout;
import java.awt.event.*;
import java.net.URL;
import java.net.MalformedURLException;
import java.io.IOException;

public class Browser {

    private JFrame frame;
    private JScrollPane scrollPane;
    private JPanel topPanel;
    private JTextField urlTextField;
    private JButton homeButton;
    private JButton goButton;
    private JEditorPane contents;
    private URL url;

    public Browser(String title) {
        
        //Initialize all components
        initializeComponents(); 

        //Set the title of frame
        frame.setTitle(title);
        
        //Set the bounds of frame
        frame.setBounds(900, 550, 1920, 1080);
        
        //Set the frame visible
        frame.setVisible(true);
        
        //Adding scrollPane to the Center of frame
        frame.add(BorderLayout.CENTER, scrollPane);

        //Adding topPanel to the North of frame
        frame.add(BorderLayout.NORTH, topPanel);

        //Adding the components urlTextField and goButton to the topPanel
        topPanel.add(homeButton);
        topPanel.add(urlTextField);
        topPanel.add(goButton);
        
    }

    private void initializeComponents() {
        //Creating the JFrame object "frame"
        frame = new JFrame();
        
        //Setting the windowClosing event to the frame
        frame.addWindowListener(new WindowAdapter(){
        public void windowClosing(WindowEvent e){
        JOptionPane.showMessageDialog(null, "Click OK to close the browser.", "Window Closing",JOptionPane.INFORMATION_MESSAGE);
        
        }
        });
        
        //Creating the JPanel object "topPanel". This one will hold both the JTextField and JButton objects
        topPanel = new JPanel();
        
        //Initializing our URL field "url"
        try 
        {
            url = new URL("https://www.cis.fiu.edu");
        }
        catch(MalformedURLException e) 
        {
            JOptionPane.showMessageDialog(null,e);
        }
        
        //Creating the JEditorPane object "contents" which accepts "url" (URL object)
        try 
        {
            contents = new JEditorPane(url);
           
            //Set editable to false
            contents.setEditable(false);
        }
        catch(IOException e) {
            JOptionPane.showMessageDialog(null,e);
        }
        
        //Adding HyperlinkListener to the JEditorPane object "contents" so that each time we click on a hyperlink the browser navigates to it.
        contents.addHyperlinkListener(new HyperlinkListener() {
            public void hyperlinkUpdate(HyperlinkEvent e) {
                if (e.getEventType() == HyperlinkEvent.EventType.ACTIVATED)
                      getThePage(e.getURL().toString());
            }
        });
        
        //Creating the JScrollPane object "scrollpane" which accepts "contents" (JEditorPane object)
        scrollPane = new JScrollPane(contents, JScrollPane.VERTICAL_SCROLLBAR_AS_NEEDED,
                JScrollPane.HORIZONTAL_SCROLLBAR_AS_NEEDED);

        //Creating the JTextField object "urlTextField"
        urlTextField = new JTextField();

        //Placing url.toString() to the JTextField object "urlTextField"
        urlTextField.setText(url.toString());
        
        //Creating the JButton object "homeButton" 
        homeButton = new JButton("HomePage");
        
        //Creating the JButton object "goButton" 
        goButton = new JButton("Go");
        
        //Adding ActionListener to the JButton object "homeButton" | When the "HomePage" button is clicked: go to the homepage: "https://www.cis.fiu.edu"
        homeButton.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
               getThePage(url.toString());
            }
        });
        
        //Adding ActionListener to the JButton object "goButton" | When the "Go" button is clicked: go to the desired URL and display its contents
        goButton.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                getThePage(urlTextField.getText());
            }
        });
    }
    
    //Create getThePage method which accepts a string "location"(url string)
    private void getThePage(String location){
        try
        {
            contents.setPage(location);       //The url string is set to the JEditorPane object "contents" so that the browser can display the contents of that page
            urlTextField.setText(location);   //The url string is set to the JTextField object "urlTextField" so that the url address changes everytime we get a page
        }
        catch(IOException ioe)
        {
            JOptionPane.showMessageDialog(null, "Error cannot access specified URL","Bad URL", JOptionPane.ERROR_MESSAGE);
        }
    }
    
}