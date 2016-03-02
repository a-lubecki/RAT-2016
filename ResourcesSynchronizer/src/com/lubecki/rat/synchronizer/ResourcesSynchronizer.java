package com.lubecki.rat.synchronizer;

import java.io.File;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.nio.file.StandardCopyOption;
import java.util.HashMap;
import java.util.Map;
import java.util.Set;

/**
 * Copyright Aurélien Lubecki 2013
 * @author Aurélien Lubecki
 */
public final class ResourcesSynchronizer {

    private final File designsDirectory;
    private final File resourcesDirectory;

    public ResourcesSynchronizer(String designsPath, String resourcesPath) {

        designsDirectory = getDirectory(designsPath);
        resourcesDirectory = getDirectory(resourcesPath);
    }

    public void sync(Map<String, Set<ExtensionTransform>> foldersToSync) {

        if(foldersToSync == null) {
            throw new IllegalArgumentException();
        }

        String designsPath = designsDirectory.getPath();
        String resourcesPath = resourcesDirectory.getPath();

        for (Map.Entry<String, Set<ExtensionTransform>> entrySet : foldersToSync.entrySet()) {

            String folderName = entrySet.getKey();
            Set<ExtensionTransform> extensionsTransform = entrySet.getValue();

            File in = getDirectory(designsPath + "/" + folderName);


            String outPath = resourcesPath + "/" + folderName;
            //create out dr if doesn't exist
            File outDir = new File(outPath);
            if(!outDir.exists()) {
                outDir.mkdir();
            }

            File out = getDirectory(outPath);

            syncFiles(in, out, extensionsTransform);
        }

    }

    public void syncFiles(File dirIn, File dirOut, Set<ExtensionTransform> extensionsTransforms) {

        //retrieve files with extension filter if set
    	Map<File, ExtensionTransform> filesToCopy = new HashMap<>();

        dirIn.listFiles((File dir, String filename) -> {

                File fileToCopy = new File(dir + "/" + filename);

                //copy only files
                if(!fileToCopy.isFile()) {
                    return false;
                }

                if(extensionsTransforms == null || extensionsTransforms.isEmpty()) {
                    filesToCopy.put(fileToCopy, null);
                    return true;
                }

                for (ExtensionTransform extensionTransform : extensionsTransforms) {
                    if(filename.endsWith(extensionTransform.inExtension)) {
                        filesToCopy.put(fileToCopy, extensionTransform);
                        return true;
                    }
                }

                return false;
            });

        //copy

        for (Map.Entry<File, ExtensionTransform> entrySet : filesToCopy.entrySet()) {

            File fileIn = entrySet.getKey();
            ExtensionTransform extensionTransform = entrySet.getValue();

            String pathIn = fileIn.getPath();
            String pathOut = dirOut.getPath() + "/";

            if(extensionTransform == null) {
                pathOut += fileIn.getName();
            } else {
                //change extension for output file
                String rootName = fileIn.getName();
                rootName = rootName.substring(0, rootName.lastIndexOf(extensionTransform.inExtension));

                pathOut += rootName + extensionTransform.outExtension;
            }

            Path in = Paths.get(pathIn);
            Path out = Paths.get(pathOut);

            if(Files.exists(out)) {
                try {
                    if(Files.getLastModifiedTime(in).equals(Files.getLastModifiedTime(out))) {
                        //the file is the same
                        continue;
                    }
                } catch (Exception ex) {
                    ex.printStackTrace();
                    //do nothing
                    System.out.println("File is the same : from " + pathIn + " to " + pathOut);
                }
            }

            try {
                Files.copy(
                        in,
                        out,
                        StandardCopyOption.REPLACE_EXISTING,
                        StandardCopyOption.COPY_ATTRIBUTES
                );
            } catch (IOException ex) {
                System.out.println("Copy failed from " + pathIn + " to " + pathOut);
                ex.printStackTrace();
            }
        }

    }


    public File getDirectory(String path) {

        if(path == null) {
            throw new IllegalArgumentException("Path null");
        }

        File dir = new File(path);

        if(!dir.exists()) {
            throw new IllegalStateException("Dir not found : " + dir.getPath());
        }
        if(!dir.isDirectory()) {
            throw new IllegalStateException("Dir not a directory : " + dir.getPath());
        }
        if(!dir.canRead()) {
            throw new IllegalStateException("Dir not readable : " + dir.getPath());
        }

        return dir;
    }

}
