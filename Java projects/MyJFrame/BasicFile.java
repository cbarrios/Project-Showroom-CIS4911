package myjframe;

import java.io.File;
import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import javax.swing.JFileChooser;

 
public class BasicFile 
{
    BasicFile f;
    File select;
    String text;
   
     
    BasicFile(String Open_Or_Save)
    {
        if("Open".equals(Open_Or_Save))
        {
            select = null;
            text = "";
            selectFile_Open();
        }
        else if("Save".equals(Open_Or_Save))
        {
            select = null;
            text = "";
            selectFile_Save();
        }
    }
     
    public void selectFile_Open() 
    {          
        try
        {
        	JFileChooser choose = new JFileChooser(".");
           	
        	int status = choose.showOpenDialog(null); //Open Dialog Box
           	
        	if (status == JFileChooser.APPROVE_OPTION) 
        	{
            	select = choose.getSelectedFile(); 
        	}
        }
        catch(NullPointerException e)
        {
        }
    }
    public void selectFile_Save() 
    {          
        try
        {
        	JFileChooser choose = new JFileChooser(".");
           	
        	int status = choose.showSaveDialog(null); //Save Dialog Box
           	
        	if (status == JFileChooser.APPROVE_OPTION) 
        	{
            	select = choose.getSelectedFile(); 
        	}
        }
        catch(NullPointerException e)
        {
        }
    }
     
    public String getName()
    {
    	//try
    //	{
    		return select.getName();
    //	}
    /*	finally
    	{
    		return "";
    	}	*/
  	}
  	 
  	public String getContent()
  	{
  		String s = "";	
  	 	
  	 	try
  	 	{
  	 		BufferedReader bf = new BufferedReader(new FileReader(select));	
  	 		while ((s = bf.readLine()) != null)
  	 			text += s + "\n";
  	 	}
  	 	catch(IOException e)
  	 	{
  	 			
  	 	}
  	 	return text;
  	}
  
}