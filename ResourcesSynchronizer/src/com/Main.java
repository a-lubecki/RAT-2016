package com;

import com.lubecki.rat.synchronizer.ExtensionTransform;
import com.lubecki.rat.synchronizer.ResourcesSynchronizer;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Map;
import java.util.Set;

/**
 * Copyright Aurélien Lubecki 2013
 * @author Aurélien Lubecki
 */
public class Main {

    public static void main(String[] args) {

        //load debug configuration
        boolean DEBUG = (args.length > 0) && "DEBUG".equals(args[0]);

        String MAIN_PATH = "./";
        if(DEBUG) {
            MAIN_PATH = "F:/RAT - Unity Project/";
        }

        final String DESIGNS_PATH = MAIN_PATH + "designs/";
        final String RESOURCES_PATH = MAIN_PATH + "RAT/Assets/Res/";

        Set<ExtensionTransform> imagesFilter = new HashSet<>();
        imagesFilter.add(new ExtensionTransform("png"));

        Set<ExtensionTransform> mapsFilter = new HashSet<>();
        mapsFilter.add(new ExtensionTransform("mm", "xml"));
        mapsFilter.add(new ExtensionTransform("json"));

        Set<ExtensionTransform> soundsFilter = new HashSet<>();
        soundsFilter.add(new ExtensionTransform("mp3"));
        soundsFilter.add(new ExtensionTransform("wav"));
        soundsFilter.add(new ExtensionTransform("ogg"));

        Map<String, Set<ExtensionTransform>> foldersToSync = new HashMap<>();

        foldersToSync.put("Characters", imagesFilter);
        foldersToSync.put("Environments", imagesFilter);
        foldersToSync.put("Maps", mapsFilter);
        foldersToSync.put("Menus", imagesFilter);
        foldersToSync.put("Musics", soundsFilter);
        foldersToSync.put("Sounds", soundsFilter);
        foldersToSync.put("Splashscreen", imagesFilter);

        //sync all
        new ResourcesSynchronizer(DESIGNS_PATH, RESOURCES_PATH).sync(foldersToSync);

    }

}
