package myjframe;

import javax.swing.JTextArea;
import javax.swing.JScrollPane;
import java.awt.Container;
import javax.swing.Action;
import javax.swing.JFrame;
import javax.swing.JMenu;
import javax.swing.JMenuBar;
import javax.swing.JMenuItem;
import javax.swing.text.DefaultEditorKit;
import java.awt.event.*;
import java.io.FileNotFoundException;
import java.io.PrintWriter;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.swing.JOptionPane;

public class DisplayText   
{   private BasicFile basicFile;
    private JTextArea text;
    private Action[] textActions_Edit = { new DefaultEditorKit.CutAction(),
         new DefaultEditorKit.CopyAction(), new DefaultEditorKit.PasteAction(),};
    private JMenuItem im;
    
   public DisplayText( String title, String info) { 
       JFrame f = new JFrame(title);
       
      Container c = f.getContentPane();

      text = new JTextArea(info, 20, 50);

      JScrollPane sp = new JScrollPane(text);
      c.add(sp);
       
       
       JMenu file = new JMenu("File");  
       JMenu edit = new JMenu("Edit");
      
        file.add(im = new JMenuItem("New"));
        im.addActionListener(new Actions());
        file.add(im = new JMenuItem("Open"));
        im.addActionListener(new Actions());
        file.addSeparator();
        file.add(im = new JMenuItem("Save"));
        im.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                if(e.getActionCommand().equalsIgnoreCase("Save")){
                basicFile = new BasicFile("Save");
                 PrintWriter pw;
                    try {
                        pw = new PrintWriter(basicFile.select);
                        
                        String info =  text.getText();
                        pw.print(info);
                        
                        pw.flush();
                        pw.close();
                        
                        JOptionPane.showMessageDialog(null, "The File Has Been Succesfully Saved!","File Saved",JOptionPane.INFORMATION_MESSAGE);
                        
                    } catch (FileNotFoundException ex) {
                        Logger.getLogger(DisplayText.class.getName()).log(Level.SEVERE, null, ex);
                    }
    
     
                }
            }
        });
        file.addSeparator();
        file.add(im = new JMenuItem("Close"));
        im.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                if(e.getActionCommand().equalsIgnoreCase("Close")){
                
                               String option = "Do you want to save this file before closing?\n1.Yes\n2.No";
                               String data = JOptionPane.showInputDialog(null,option,"Window Closing",JOptionPane.QUESTION_MESSAGE);
                               int selection = Integer.parseInt(data);
                              switch(selection){
                              case 1:
                                    basicFile = new BasicFile("Save");
                                    PrintWriter pw;
                                     try {
                                        pw = new PrintWriter(basicFile.select);
                        
                                        String info =  text.getText();
                                        pw.print(info);
                                        
                                        pw.flush();
                                        pw.close();
                        
                                    JOptionPane.showMessageDialog(null, "The File Has Been Succesfully Saved!","File Saved",JOptionPane.INFORMATION_MESSAGE);
                                    
                                    f.dispose(); //dispose after saving
                    } catch (FileNotFoundException ex) {
                        Logger.getLogger(DisplayText.class.getName()).log(Level.SEVERE, null, ex);
                    }
                                  break;
                              case 2:
                                  f.dispose(); 
                                  break;
                              default: JOptionPane.showMessageDialog(null,"Please Select Between Option 1 or 2 Only!","Invalid Input",JOptionPane.INFORMATION_MESSAGE); break;
        }
                }
            }
        });
   
      for (Action textAction : textActions_Edit) {
         edit.add(new JMenuItem(textAction));
      }
      
      JMenuBar menubar = new JMenuBar();
      menubar.add(file);
      menubar.add(edit);

      f.setJMenuBar(menubar);

      f.pack();
      f.addWindowListener(new WindowAdapter(){
        public void windowClosing(WindowEvent e){
             String option = "Do you want to save this file before closing?\n1.Yes\n2.No";
                               String data = JOptionPane.showInputDialog(null,option,"Window Closing",JOptionPane.QUESTION_MESSAGE);
                               int selection = Integer.parseInt(data);
                          switch(selection){
                              case 1:
                                    basicFile = new BasicFile("Save");
                                    PrintWriter pw;
                                     try {
                                        pw = new PrintWriter(basicFile.select);
                        
                                        String info =  text.getText();
                                        pw.print(info);
       
                                        pw.flush();
                                        pw.close();
                                        
                                        
                                    JOptionPane.showMessageDialog(null, "The File Has Been Succesfully Saved!","File Saved",JOptionPane.INFORMATION_MESSAGE);
                                    f.dispose();
                    } catch (FileNotFoundException ex) {
                        Logger.getLogger(DisplayText.class.getName()).log(Level.SEVERE, null, ex);
                    }
                                  break;
                              case 2:
                                  f.dispose(); 
                                  break;
                              default: JOptionPane.showMessageDialog(null,"Please Select Between Option 1 or 2 Only!","Invalid Input",JOptionPane.INFORMATION_MESSAGE); break;
        
        }
        }});
      
      f.setDefaultCloseOperation(JFrame.DO_NOTHING_ON_CLOSE);
      f.setLocationRelativeTo(null);
      f.setVisible(true);
   }
  
   }
