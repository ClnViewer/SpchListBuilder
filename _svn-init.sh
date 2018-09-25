#!/bin/sh

SVNCMD="sudo -u admin svn"

${SVNCMD} propset svn:ignore -RF .ignoresvn .
