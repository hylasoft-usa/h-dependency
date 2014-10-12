'use strict';

module.exports = function(grunt) {

  // Load grunt tasks automatically
  require('load-grunt-tasks')(grunt);

  // Time how long tasks take. Can help when optimizing build times
  require('time-grunt')(grunt);


  grunt.initConfig({

    //set this variables for different projects
    srcPath: 'h-dependency/',
    productName: 'h-dependency',
    dotNetVersion: '4.5.0',
    platform: 'Any CPU',


    pkg: grunt.file.readJSON('package.json'),

    assemblyinfo: {
      options: {
        files: ['<%= srcPath %>h-dependency.sln'],
        info: {
          version: '<%= pkg.version %>',
          fileVersion: '<%= pkg.version %>',
          company: 'hylasoft',
          copyright: ' ',
          product: '<%= productName %>',
          title: '<%= productName %>'
        }
      }
    },

    msbuild: {
      release: {
        src: ['<%= srcPath %>h-dependency.sln'],
        options: {
          projectConfiguration: 'Release',
          platform: '<%= platform %>',
          targets: ['Clean', 'Rebuild'],
          stdout: true
        }
      },
      debug: {
        src: ['<%= srcPath %>h-dependency.sln'],
        options: {
          projectConfiguration: 'Debug',
          platform: '<%= platform %>',
          targets: ['Clean', 'Rebuild'],
          stdout: true
        }
      }
    },

    mstest: {
      debug: {
        src: ['<%= srcPath %>/**/bin/Release/*.dll'] // Points to test dll
      }
    }

  });
  grunt.registerTask('default', ['build']);

  grunt.registerTask('build', ['msbuild:release']);
  grunt.registerTask('test', ['msbuild:release', 'mstest']);

}
